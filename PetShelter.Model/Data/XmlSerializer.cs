using PetShelter.Model.Core;

namespace PetShelter.Model.Data
{
    public class XmlSerializer : SerializerBase
    {
        public XmlSerializer() : base()
        {
        }
        
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
                namespaces.Add(string.Empty, string.Empty); // Убираем лишние пространства имен
                
                serializer.Serialize(writer, data, namespaces);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сериализации XML: {ex.Message}");
                throw;
            }
        }
        
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
}