namespace PetShelter.Model.Core;

public abstract partial class Pet
{
    public abstract Type Type { get; }
    public string Name { get; private set; }
    public int Age { get; private set; }
    public double Weight { get; private set; }

    public Pet(string name, int age, double weight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым", nameof(name));
        if (age <= 0)
            throw new ArgumentException("Возраст должен быть положительным", nameof(age));
        if (weight <= 0)
            throw new ArgumentException("Вес должен быть положительным", nameof(weight));

        Name = name;
        Age = age;
        Weight = weight;
    }
}

public abstract partial class Pet
{
    public bool Claustrophobic { get; private set; } // Исправлено имя свойства

    public Pet(string name, int age, double weight, bool claustrophobic)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым", nameof(name));
        if (age <= 0)
            throw new ArgumentException("Возраст должен быть положительным", nameof(age));
        if (weight <= 0)
            throw new ArgumentException("Вес должен быть положительным", nameof(weight));

        Name = name;
        Age = age;
        Weight = weight;
        Claustrophobic = claustrophobic;
    }

    public override string ToString()
    {
        return $"Тип: {Type}, Имя: {Name}, Возраст: {Age}, Вес: {Weight}, " +
               $"Клаустрофобия: {(Claustrophobic ? "Да" : "Нет")}";
    }
}