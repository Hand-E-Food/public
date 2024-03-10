using Newtonsoft.Json;
using SpaceTraders.Api;

namespace SpaceTraders
{
    public class AgentRepository
    {
        private readonly ApiClient apiClient;

        public AgentRepository(ApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<GameState> GetOrRegister(string agentSymbol)
        {
            var gameState = await Get(agentSymbol);
            if (gameState == null)
            {
                gameState = await Register(agentSymbol);
                await Persist(gameState);
            }
            return gameState;
        }

        public async Task<GameState?> Get(string agentSymbol)
        {
            string path = GetPath(agentSymbol);
            if (!File.Exists(path)) return null;
            var json = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<GameState>(json);
        }

        public async Task<GameState> Register(string agentSymbol)
        {
            var response = await apiClient.RegisterAsync(new()
            {
                Email = "hand_e_food@hotmail.com",
                Faction = FactionSymbols.COSMIC,
                Symbol = agentSymbol,
            });

            var data = response.Data;

            return new GameState
            {
                Agent = data.Agent,
                Contract = data.Contract,
                Faction = data.Faction,
                Ships = { data.Ship },
                Token = data.Token,
            };
        }

        public async Task Persist(GameState gameState)
        {
            var json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            Directory.CreateDirectory("saves");
            await File.WriteAllTextAsync(GetPath(gameState.Agent.Symbol), json);
        }

        private string GetPath(string agentSymbol) => Path.Join("saves", agentSymbol + ".json");
    }
}
