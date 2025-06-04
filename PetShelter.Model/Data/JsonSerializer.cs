using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PetShelter.Model.Data;

/// <summary>
/// Класс для сериализации и десериализации объектов в формате JSON.
/// Наследуется от абстрактного класса SerializerBase.
/// </summary>
public class JsonSerializer : SerializerBase
{
    /// <summary>
    /// Настройки сериализации JSON.
    /// </summary>
    private readonly JsonSerializerSettings _settings;

    /// <summary>
    /// Конструктор класса JsonSerializer.
    /// Инициализирует настройки JSON с форматированием и сохранением информации о типе.
    /// </summary>
    public JsonSerializer() : base()
    {
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects, // Сохраняем информацию о типе
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    /// <summary>
    /// Сериализует объект в JSON-строку и сохраняет в файл.
    /// </summary>
    /// <typeparam name="T">Тип сериализуемого объекта</typeparam>
    /// <param name="fileName">Имя файла без расширения</param>
    /// <param name="data">Объект для сериализации</param>
    public override void Serialize<T>(string fileName, T data)
    {
        var json = JsonConvert.SerializeObject(data, _settings);
        var fullPath = GetFullPath($"{fileName}.json");
        
        // Создание файла, если он не существует
        EnsureFileExists(fullPath);
        File.WriteAllText(fullPath, json);
    }

    /// <summary>
    /// Десериализует объект из JSON-файла.
    /// </summary>
    /// <typeparam name="T">Тип десериализуемого объекта</typeparam>
    /// <param name="fileName">Имя файла (с расширением .json или без него)</param>
    /// <returns>Десериализованный объект типа T</returns>
    /// <exception cref="FileNotFoundException">
    /// Выбрасывается, если файл не найден
    /// </exception>
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
        
        // Десериализация JSON-строки в объект типа T
        return JsonConvert.DeserializeObject<T>(json, _settings);
    }
}