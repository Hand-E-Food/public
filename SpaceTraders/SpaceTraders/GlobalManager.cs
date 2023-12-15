using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpaceTraders.Api;
using SpaceTraders.Model;
using System.Text;

namespace SpaceTraders
{
    public class GlobalManager
    {
        private readonly ApiClient apiClient;
        private readonly IGlobalStateRepository globalStateRepository;
        private readonly ILogger logger;
        private readonly Paginator paginator;
        private readonly IServiceProvider serviceProvider;

        public Response Status { get; private set; } = null!;
        public GlobalState GlobalState { get; private set; } = null!;

        public GlobalManager(
            ILogger<GlobalManager> logger,
            ApiClient apiClient,
            IGlobalStateRepository globalStateRepository,
            Paginator paginator,
            IServiceProvider serviceProvider
        ) {
            this.logger = logger;
            this.apiClient = apiClient;
            this.globalStateRepository = globalStateRepository;
            this.paginator = paginator;
            this.serviceProvider = serviceProvider;
        }

        public async Task Initialise()
        {
            Status = await GetStatus();
            var globalState = await globalStateRepository.Get();
            if (globalState != null && globalState.ResetDate == Status.ResetDate)
            {
                GlobalState = globalState;
                return;
            }

            logger.LogInformation("Server has reset. Reloading data.");
            await globalStateRepository.Clear();
            var factions = await GetFactions();
            var systems = await GetSystems();

            globalState = new() {
                Factions = factions
                    .ToDictionary(faction => faction.Symbol),
                ResetDate = Status.ResetDate,
                Sectors = systems
                    .GroupBy(system => system.SectorSymbol)
                    .Select(group => new Sector
                    {
                        Symbol = group.Key,
                        Systems = group.ToDictionary(system => system.Symbol),
                    })
                    .ToDictionary(sector => sector.Symbol),
                Systems = systems
                    .ToDictionary(system => system.Symbol)
            };

            await globalStateRepository.Persist(globalState);
        }

        private async Task<Response> GetStatus()
        {
            logger.LogDebug("Getting SpaceTraders API status.");
            var status = await apiClient.GetStatusAsync();
            var message = new StringBuilder()
                .Append("SpaceTraders ")
                .AppendLine(status.Version)
                .AppendLine(status.Status)
                .Append("Server was last reset at ")
                .Append(status.ResetDate)
                .Append(" and will next reset at ")
                .Append(status.ServerResets.Next)
                .Append(" on a ")
                .Append(status.ServerResets.Frequency)
                .AppendLine(" cadence.");
            logger.LogInformation(message.ToString());

            return status;
        }

        private async Task<List<Faction>> GetFactions()
        {
            logger.LogInformation("Loading factions.");
            var factions = await paginator.GetAll(apiClient.GetFactionsAsync, r => r.Data);
            logger.LogInformation("Loaded {Count} factions.", factions.Count);
            return factions;
        }

        private async Task<List<StarSystem>> GetSystems()
        {
            logger.LogInformation("Loading systems.");
            var systems = await paginator.GetAll(apiClient.GetSystemsAsync, r => r.Data);
            logger.LogInformation("Loaded {Count} systems.", systems.Count);
            return systems;
        }

        public async Task<AgentScope> CreateAgentScope(string agentSymbol)
        {
            var agentRepository = serviceProvider.GetRequiredService<AgentRepository>();
            var gameState = await agentRepository.GetOrRegister(agentSymbol);
            var logger = serviceProvider.GetRequiredService<ILogger<AgentScope>>();
            var serviceScope = serviceProvider.CreateScope();
            try
            {
                return new AgentScope(logger, agentRepository, gameState, serviceScope);
            }
            catch
            {
                serviceScope.Dispose();
                throw;
            }
        }
    }
}
