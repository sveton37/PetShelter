using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace PetShelter.Model.Data
{
    public class JsonSerializer : SerializerBase
    {
        private readonly JsonSerializerSettings _settings;
        
        public JsonSerializer() : base()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects, // Сохраняем информацию о типе
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        
        public override void Serialize<T>(string fileName, T data)
        {
            var json = JsonConvert.SerializeObject(data, _settings);
            var fullPath = GetFullPath($"{fileName}.json");
            EnsureFileExists(fullPath);
            File.WriteAllText(fullPath, json);
        }
        
        public override T Deserialize<T>(string fileName)
        {
            var fullPath = fileName.EndsWith(".json") 
                ? GetFullPath(fileName) 
                : GetFullPath($"{fileName}.json");
                
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Файл {fullPath} не найден");
            }
            
            var json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}