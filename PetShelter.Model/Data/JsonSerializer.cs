using Newtonsoft.Json;

namespace PetShelter.Model.Data;

public class JsonSerializer : SerializerBase
{
    public readonly JsonSerializerSettings _settings;

    public JsonSerializer()
    {
        _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
    }

    public override void Serialize<T>(string fileName, T data)
    {
        var path = GetFullPath($"{fileName}.json");
        EnsureFileExists(path);
        string jsonData = JsonConvert.SerializeObject(data, _settings);
        File.WriteAllText(path, jsonData);
    }

    public override T Deserialize<T>(string fileName)
    {
        var path = GetFullPath($"{fileName}.json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File {fileName}.json not found");
        }

        string jsonData = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(jsonData, _settings);
    }
}