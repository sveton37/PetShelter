using Newtonsoft.Json;
using System.Xml.Serialization;

namespace PetShelter.Model.Core;

[XmlInclude(typeof(Dog))]
[XmlInclude(typeof(Cat))]
[XmlInclude(typeof(Rabbit))]
public abstract partial class Pet
{
    // Необходимый пустой конструктор для XML-сериализации
    protected Pet() { }
    
    [XmlIgnore] // Свойство Type не будет сериализоваться
    public abstract Type Type { get; }
    
    [XmlElement("Name")]
    public string Name { get; set; } // Изменили на публичный сеттер
    
    [XmlElement("Age")]
    public int Age { get; set; } // Изменили на публичный сеттер
    
    [XmlElement("Weight")]
    public double Weight { get; set; } // Изменили на публичный сеттер

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
    [XmlElement("Claustrophobic")]
    public bool Claustrophobic { get; set; } // Изменили на публичный сеттер
    
    [JsonProperty("PetType")]
    [XmlElement("PetType")]
    public string PetType => this.GetType().Name;

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

    public string ClaustrophobicText => Claustrophobic ? "Да" : "Нет";
}