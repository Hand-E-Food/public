using Newtonsoft.Json;

namespace SpaceTraders.Cli
{
    public class JsonStorage
    {
        private readonly string basePath = "saves";

        public Task Clear()
        {
            Directory.Delete(basePath, recursive: true);
            return Task.CompletedTask;
        }

        public async Task<T?> Get<T>(string name)
        {
            var path = GetFullPath(name);
            if (!File.Exists(path)) return default;
            var json = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task Put(string name, object dto)
        {
            var path = GetFullPath(name);
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            Directory.CreateDirectory(basePath);
            await File.WriteAllTextAsync(path, json);
        }

        private string GetFullPath(string name) => Path.Join(basePath, name + ".json");
    }
}
