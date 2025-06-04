namespace PetShelter.Model.Data;

/// <summary>
/// Абстрактный базовый класс для всех сериализаторов.
/// Определяет общую функциональность и интерфейс для различных форматов сериализации.
/// </summary>
public abstract class SerializerBase
{
    /// <summary>
    /// Базовая директория для хранения сериализованных файлов.
    /// </summary>
    protected string BaseDirectory { get; }

    /// <summary>
    /// Конструктор класса SerializerBase.
    /// Создает директорию для хранения данных, если она не существует.
    /// </summary>
    protected SerializerBase()
    {
        BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
    }

    /// <summary>
    /// Абстрактный метод для сериализации объекта в файл.
    /// </summary>
    /// <typeparam name="T">Тип сериализуемого объекта</typeparam>
    /// <param name="fileName">Имя файла без расширения</param>
    /// <param name="data">Объект для сериализации</param>
    public abstract void Serialize<T>(string fileName, T data);
    
    /// <summary>
    /// Абстрактный метод для десериализации объекта из файла.
    /// </summary>
    /// <typeparam name="T">Тип десериализуемого объекта</typeparam>
    /// <param name="fileName">Имя файла</param>
    /// <returns>Десериализованный объект типа T</returns>
    public abstract T Deserialize<T>(string fileName);

    /// <summary>
    /// Получает полный путь к файлу в базовой директории.
    /// </summary>
    /// <param name="fileName">Имя файла</param>
    /// <returns>Полный путь к файлу</returns>
    protected string GetFullPath(string fileName)
    {
        return Path.Combine(BaseDirectory, fileName);
    }

    /// <summary>
    /// Создает файл, если он не существует.
    /// </summary>
    /// <param name="path">Полный путь к файлу</param>
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