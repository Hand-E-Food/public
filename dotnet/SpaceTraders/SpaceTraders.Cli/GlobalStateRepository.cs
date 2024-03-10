using Microsoft.Extensions.Logging;
using SpaceTraders.Cli.DataModel;
using SpaceTraders.Model;

namespace SpaceTraders.Cli
{
    internal class GlobalStateRepository : IGlobalStateRepository
    {
        private const string Name = "global-state";

        private readonly JsonStorage jsonStorage;
        private readonly ILogger logger;

        public GlobalStateRepository(ILogger<GlobalStateRepository> logger, JsonStorage jsonStorage)
        {
            this.logger = logger;
            this.jsonStorage = jsonStorage;
        }

        public Task Clear() => jsonStorage.Clear();

        public async Task<GlobalState?> Get()
        {
            var dto = await jsonStorage.Get<GlobalStateDto>(Name);
            if (dto == null) return null;

            return new()
            {
                Factions = dto.Factions
                    .ToDictionary(faction => faction.Symbol),
                ResetDate = dto.ResetDate,
                Sectors = dto.Systems
                    .GroupBy(system => system.SectorSymbol)
                    .Select(group => new Sector {
                        Symbol = group.Key,
                        Systems = group.ToDictionary(system => system.Symbol),
                    })
                    .ToDictionary(sector => sector.Symbol),
                Systems = dto.Systems
                    .ToDictionary(system => system.Symbol),
            };
        }

        public async Task Persist(GlobalState globalState)
        {
            var dto = new GlobalStateDto {
                Factions = globalState.Factions.Values,
                ResetDate = globalState.ResetDate,
                Systems = globalState.Systems.Values,
            };
            await jsonStorage.Put(Name, dto);
        }
    }
}
