namespace PetShelter.Model.Data;

public class XmlSerializer : SerializerBase
{
    public override void Serialize<T>(string fileName, T data)
    {
        var path = GetFullPath($"{fileName}.xml");
        EnsureFileExists(path);

        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (var writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, data);
        }
    }

    public override T Deserialize<T>(string fileName)
    {
        var path = GetFullPath($"{fileName}.xml");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File {fileName}.xml not found");
        }

        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (var reader = new StreamReader(path))
        {
            return (T)serializer.Deserialize(reader);
        }
    }
}