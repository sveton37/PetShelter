using PetShelter.Model.Core;

namespace PetShelter.Model.Data;

/// <summary>
/// Класс для сериализации и десериализации объектов в формате XML.
/// Наследуется от абстрактного класса SerializerBase.
/// </summary>
public class XmlSerializer : SerializerBase
{
    /// <summary>
    /// Конструктор класса XmlSerializer.
    /// </summary>
    public XmlSerializer() : base()
    {
    }

    /// <summary>
    /// Сериализует объект в XML-формат и сохраняет в файл.
    /// </summary>
    /// <typeparam name="T">Тип сериализуемого объекта</typeparam>
    /// <param name="fileName">Имя файла без расширения</param>
    /// <param name="data">Объект для сериализации</param>
    /// <exception cref="Exception">
    /// Перехватывает и логирует ошибки сериализации, затем пробрасывает их дальше
    /// </exception>
    public override void Serialize<T>(string fileName, T data)
    {
        try
        {
            // Создаем XmlSerializer с известными типами
            var knownTypes = new Type[]
            {
                typeof(Pet),
                typeof(Dog),
                typeof(Cat),
                typeof(Rabbit)
            };

            var fullPath = GetFullPath($"{fileName}.xml");
            EnsureFileExists(fullPath);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), knownTypes);
            using var writer = new StreamWriter(fullPath);

            // Добавляем настройки XML для правильной обработки
            var namespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
            // Убираем лишние пространства имен
            namespaces.Add(string.Empty, string.Empty); // Убираем лишние пространства имен

            serializer.Serialize(writer, data, namespaces);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сериализации XML: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Десериализует объект из XML-файла.
    /// </summary>
    /// <typeparam name="T">Тип десериализуемого объекта</typeparam>
    /// <param name="fileName">Имя файла (с расширением .xml или без него)</param>
    /// <returns>Десериализованный объект типа T</returns>
    /// <exception cref="FileNotFoundException">
    /// Выбрасывается, если файл не найден
    /// </exception>
    /// <exception cref="Exception">
    /// Перехватывает и логирует ошибки десериализации, затем пробрасывает их дальше
    /// </exception>
    public override T Deserialize<T>(string fileName)
    {
        try
        {
            var fullPath = fileName.EndsWith(".xml")
                ? GetFullPath(fileName)
                : GetFullPath($"{fileName}.xml");

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Файл {fullPath} не найден");
            }

            // Создаем XmlSerializer с известными типами
            var knownTypes = new Type[]
            {
                typeof(Pet),
                typeof(Dog),
                typeof(Cat),
                typeof(Rabbit)
            };

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), knownTypes);
            using var reader = new StreamReader(fullPath);
            return (T)serializer.Deserialize(reader);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка десериализации XML: {ex.Message}");
            throw;
        }
    }
}