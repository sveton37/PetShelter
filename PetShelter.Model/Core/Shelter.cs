using PetShelter.Model.Core.Interfaces;
using PetShelter.Model.Data;

namespace PetShelter.Model.Core;

public partial class Shelter : ICountable, IFilter
{
    private readonly List<Pet> _pets = new();
    public string Name { get; }
    public int Capacity { get; }
    public bool HasOpenYard { get; }

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

    public int Count() => _pets.Count;

    public int Count(Type petType) => 
        _pets.Count(p => p.GetType() == petType);

    public int Percentage(Type petType) => 
        _pets.Count == 0 ? 0 : (Count(petType) * 100) / Count();

    public IEnumerable<Pet> Filter(Type petType) => 
        _pets.Where(p => p.GetType() == petType);

    public void AddPet(Pet pet)
    {
        
        if (_pets.Count >= Capacity)
            throw new InvalidOperationException("Приют переполнен");
        
        if (pet.Claustrophobic && !HasOpenYard)
            throw new InvalidOperationException("Нельзя поместить клаустрофобное животное в приют без открытой территории");

        _pets.Add(pet);
    }
    
    public bool RemovePet(Pet pet) => _pets.Remove(pet);

    public IEnumerable<Pet> Filter(Type petType, bool onlyClaustrophobic)
    {
        var filtered = Filter(petType);
        return onlyClaustrophobic ? 
            filtered.Where(p => p.Claustrophobic) : 
            filtered;
    }
}

public partial class Shelter : IChangeable
{
    public Pet Add(Pet pet)
    {
        if (pet == null)
            throw new ArgumentNullException(nameof(pet));

        if (_pets.Count >= Capacity)
            throw new InvalidOperationException("Приют переполнен");

        if (pet.Claustrophobic && !HasOpenYard)
            throw new InvalidOperationException("Нельзя поместить клаустрофобное животное в приют без открытой территории");

        _pets.Add(pet);
        SavePetToFiles(pet);
        return pet;
    }

    public Pet Remove(Pet pet)
    {
        if (pet == null)
            throw new ArgumentNullException(nameof(pet));

        if (!_pets.Remove(pet))
            throw new InvalidOperationException("Животное не найдено в приюте");

        return pet;
    }
    
    private void SavePetToFiles(Pet pet)
    {
        var fileName = $"pet_{pet.GetType().Name}_{DateTime.Now:yyyyMMddHHmmss}";
        var jsonSerializer = new JsonSerializer();
        var xmlSerializer = new XmlSerializer();
        
        jsonSerializer.Serialize(fileName, pet);
        xmlSerializer.Serialize(fileName, pet);
    }

}