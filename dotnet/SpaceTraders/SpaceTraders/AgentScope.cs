using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SpaceTraders
{
    public class AgentScope : IServiceScope
    {
        private readonly AgentRepository agentRepository;
        private readonly ILogger logger;
        private readonly IServiceScope serviceScope;
        private bool isDisposed;

        public GameState GameState { get; }
        public IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        public AgentScope(
            ILogger<AgentScope> logger,
            AgentRepository agentRepository,
            GameState gameState,
            IServiceScope serviceScope
        ) {
            this.logger = logger;
            this.agentRepository = agentRepository;
            this.GameState = gameState;
            this.serviceScope = serviceScope;

            var httpClient = ServiceProvider.GetRequiredService<HttpClient>();
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", gameState.Token);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    agentRepository.Persist(GameState).Wait();
                    serviceScope.Dispose();
                }
                isDisposed = true;
            }
        }
    }
}
