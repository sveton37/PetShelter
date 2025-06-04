using PetShelter.Model.Core.Interfaces;
using PetShelter.Model.Data;

namespace PetShelter.Model.Core;

/// <summary>
/// Класс, представляющий приют для животных.
/// Реализует интерфейсы ICountable и IFilter для подсчета и фильтрации животных.
/// </summary>
/// <remarks>
/// Класс разделен на две части с помощью partial для разделения основной функциональности
/// и реализации интерфейса IChangeable.
/// </remarks>
public partial class Shelter : ICountable, IFilter
{
    /// <summary>
    /// Список животных, содержащихся в приюте.
    /// </summary>
    private readonly List<Pet> _pets = new();
    
    /// <summary>
    /// Название приюта.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Максимальная вместимость приюта (количество животных).
    /// </summary>
    public int Capacity { get; }
    
    /// <summary>
    /// Флаг, указывающий, имеет ли приют открытую территорию.
    /// </summary>
    /// <remarks>
    /// Животные с клаустрофобией могут содержаться только в приютах с открытой территорией.
    /// </remarks>
    public bool HasOpenYard { get; }

    /// <summary>
    /// Конструктор класса Shelter.
    /// </summary>
    /// <param name="name">Название приюта (не может быть пустым)</param>
    /// <param name="capacity">Вместимость приюта (должна быть положительной)</param>
    /// <param name="hasOpenYard">Наличие открытой территории</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если название пустое или вместимость не положительная.
    /// </exception>
    public Shelter(string name, int capacity, bool hasOpenYard)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя приюта не может быть пустым", nameof(name));
        if (capacity <= 0)
            throw new ArgumentException("Вместимость должна быть положительной", nameof(capacity));

        Name = name;
        Capacity = capacity;
        HasOpenYard = hasOpenYard;
    }

    /// <summary>
    /// Возвращает общее количество животных в приюте.
    /// </summary>
    /// <returns>Количество животных</returns>
    public int Count() => _pets.Count;

    /// <summary>
    /// Возвращает количество животных определенного типа в приюте.
    /// </summary>
    /// <param name="petType">Тип животного для подсчета</param>
    /// <returns>Количество животных указанного типа</returns>
    public int Count(Type petType) => 
        _pets.Count(p => p.GetType() == petType); 

    /// <summary>
    /// Вычисляет процентное соотношение животных определенного типа к общему количеству.
    /// </summary>
    /// <param name="petType">Тип животного для расчета процента</param>
    /// <returns>
    /// Процент животных указанного типа от общего количества.
    /// Возвращает 0, если приют пуст.
    /// </returns>
    public int Percentage(Type petType) => 
        _pets.Count == 0 ? 0 : (Count(petType) * 100) / Count();

    /// <summary>
    /// Фильтрует животных по типу.
    /// </summary>
    /// <param name="petType">Тип животного для фильтрации</param>
    /// <returns>
    /// Коллекция животных указанного типа.
    /// Если тип не указан или равен базовому типу Pet, возвращаются все животные.
    /// </returns>
    public IEnumerable<Pet> Filter(Type petType)
    {
        if (petType == null || petType == typeof(Pet))
        {
            return _pets.ToList();
        }
        
        // Проверяем с помощью свойства Type, которое определено в каждом классе животных
        return _pets.Where(pet => pet.Type == petType).ToList();
    }

    /// <summary>
    /// Добавляет животное в приют с проверками на вместимость и клаустрофобию.
    /// </summary>
    /// <param name="pet">Животное для добавления</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если приют переполнен или 
    /// животное с клаустрофобией помещается в приют без открытой территории.
    /// </exception>
    public void AddPet(Pet pet)
    {
        
        if (_pets.Count >= Capacity)
            throw new InvalidOperationException("Приют переполнен");
        
        if (pet.Claustrophobic && !HasOpenYard)
            throw new InvalidOperationException("Нельзя поместить клаустрофобное животное в приют без открытой территории");

        _pets.Add(pet);
    }
    
    /// <summary>
    /// Удаляет животное из приюта.
    /// </summary>
    /// <param name="pet">Животное для удаления</param>
    /// <returns>true, если животное было удалено; иначе false</returns>
    public bool RemovePet(Pet pet) => _pets.Remove(pet);

    /// <summary>
    /// Фильтрует животных по типу с возможностью дополнительной фильтрации по клаустрофобии.
    /// </summary>
    /// <param name="petType">Тип животного для фильтрации</param>
    /// <param name="onlyClaustrophobic">
    /// Если true, возвращаются только животные с клаустрофобией;
    /// если false, возвращаются все животные указанного типа
    /// </param>
    /// <returns>Отфильтрованная коллекция животных</returns>
    public IEnumerable<Pet> Filter(Type petType, bool onlyClaustrophobic)
    {
        var filtered = Filter(petType);
        return onlyClaustrophobic ? 
            filtered.Where(p => p.Claustrophobic) : 
            filtered;
    }
}

/// <summary>
/// Вторая часть частичного класса Shelter, реализующая интерфейс IChangeable.
/// Предоставляет расширенную функциональность для добавления и удаления животных
/// с сохранением данных в файлы.
/// </summary>
public partial class Shelter : IChangeable
{
    /// <summary>
    /// Добавляет животное в приют с дополнительными проверками и сохранением в файлы.
    /// </summary>
    /// <param name="pet">Животное для добавления</param>
    /// <returns>Добавленное животное</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если pet равен null</exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если приют переполнен, животное не может быть помещено из-за клаустрофобии,
    /// или животное с такими параметрами уже существует в приюте.
    /// </exception>
    public Pet Add(Pet pet)
    {
        if (pet == null)
            throw new ArgumentNullException(nameof(pet));

        if (_pets.Count >= Capacity)
            throw new InvalidOperationException("Приют переполнен");

        if (pet.Claustrophobic && !HasOpenYard)
            throw new InvalidOperationException("Нельзя поместить клаустрофобное животное в приют без открытой территории");

        // Проверка на уникальность питомца
        if (_pets.Any(p => 
            p.Name.Equals(pet.Name, StringComparison.OrdinalIgnoreCase) &&
            p.Age == pet.Age &&
            Math.Abs(p.Weight - pet.Weight) < 0.01 &&
            p.Type == pet.Type))
        {
            throw new InvalidOperationException("Животное с такими параметрами уже существует в приюте");
        }

        _pets.Add(pet);
        SavePetToFiles(pet);
        return pet;
    }

    /// <summary>
    /// Удаляет животное из приюта.
    /// </summary>
    /// <param name="pet">Животное для удаления</param>
    /// <returns>Удаленное животное</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если pet равен null</exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если животное не найдено в приюте
    /// </exception>
    public Pet Remove(Pet pet)
    {
        if (pet == null)
            throw new ArgumentNullException(nameof(pet));

        if (!_pets.Remove(pet))
            throw new InvalidOperationException("Животное не найдено в приюте");

        return pet;
    }
    
    /// <summary>
    /// Сохраняет информацию о животном в файлы в форматах JSON и XML.
    /// </summary>
    /// <param name="pet">Животное для сохранения</param>
    /// <remarks>
    /// Метод обрабатывает исключения, чтобы предотвратить прерывание процесса добавления
    /// в случае ошибки сохранения файлов.
    /// </remarks>
    private void SavePetToFiles(Pet pet)
    {
        try
        {
            var fileName = $"pet_{pet.GetType().Name}_{DateTime.Now:yyyyMMddHHmmss}";
            
            // Сохраняем в JSON 
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(fileName, pet);
            
            // Используем XML-сериализацию
            var xmlSerializer = new XmlSerializer();
            xmlSerializer.Serialize(fileName, pet);
        }
        catch (Exception ex)
        {
            // Обрабатываем ошибку, но не позволяем ей прервать добавление питомца
            Console.WriteLine($"Ошибка при сохранении файлов питомца: {ex.Message}");
        }
    }
}