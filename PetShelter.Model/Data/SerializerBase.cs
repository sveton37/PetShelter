namespace PetShelter.Model.Data;

public abstract class SerializerBase
{
    protected string BaseDirectory { get; }

    protected SerializerBase()
    {
        BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
    }

    public abstract void Serialize<T>(string fileName, T data);
    public abstract T Deserialize<T>(string fileName);

    protected string GetFullPath(string fileName)
    {
        return Path.Combine(BaseDirectory, fileName);
    }

    protected void EnsureFileExists(string path)
    {
        if (!File.Exists(path))
        {
            using (File.Create(path))
            {
            }
        }
    }
}