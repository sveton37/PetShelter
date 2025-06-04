using Newtonsoft.Json;
using System.Xml.Serialization;

namespace PetShelter.Model.Core;

/// <summary>
/// Абстрактный базовый класс для всех типов животных.
/// Определяет основные свойства и поведение, общие для всех питомцев.
/// Разбит на partial-классы для разделения базовой функциональности и дополнительных свойств.
/// </summary>
/// <remarks>
/// Поддерживает XML и JSON сериализацию с использованием атрибутов.
/// Атрибуты XmlInclude указывают сериализатору, какие конкретные классы могут быть десериализованы.
/// </remarks>
[XmlInclude(typeof(Dog))]
[XmlInclude(typeof(Cat))]
[XmlInclude(typeof(Rabbit))]
public abstract partial class Pet
{
    /// <summary>
    /// Пустой защищенный конструктор, необходимый для XML-сериализации.
    /// </summary>
    /// <remarks>
    /// Этот конструктор используется только механизмом десериализации
    /// и не должен вызываться непосредственно из кода.
    /// </remarks>
    protected Pet() { }
    
    /// <summary>
    /// Абстрактное свойство, представляющее тип животного.
    /// </summary>
    /// <remarks>
    /// Должно быть переопределено в каждом конкретном классе животного.
    /// Не сериализуется в XML благодаря атрибуту XmlIgnore.
    /// </remarks>
    [XmlIgnore]
    public abstract Type Type { get; }
    
    /// <summary>
    /// Имя животного.
    /// </summary>
    /// <remarks>
    /// При сериализации в XML будет использовано имя элемента "Name".
    /// Свойство имеет публичный сеттер для поддержки сериализации.
    /// </remarks>
    [XmlElement("Name")]
    public string Name { get; set; } 
    
    /// <summary>
    /// Возраст животного в годах.
    /// </summary>
    /// <remarks>
    /// При сериализации в XML будет использовано имя элемента "Age".
    /// Свойство имеет публичный сеттер для поддержки сериализации.
    /// </remarks>
    [XmlElement("Age")]
    public int Age { get; set; } 
    
    /// <summary>
    /// Вес животного в килограммах.
    /// </summary>
    /// <remarks>
    /// При сериализации в XML будет использовано имя элемента "Weight".
    /// Свойство имеет публичный сеттер для поддержки сериализации.
    /// </remarks>
    [XmlElement("Weight")]
    public double Weight { get; set; } 

    /// <summary>
    /// Конструктор с базовыми параметрами для создания животного.
    /// </summary>
    /// <param name="name">Имя животного (не может быть пустым)</param>
    /// <param name="age">Возраст животного (должен быть положительным)</param>
    /// <param name="weight">Вес животного (должен быть положительным)</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если какой-либо из параметров имеет недопустимое значение.
    /// </exception>
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

/// <summary>
/// Вторая часть частичного класса Pet, содержащая дополнительные свойства и методы.
/// </summary>
public abstract partial class Pet
{
    /// <summary>
    /// Флаг, указывающий, страдает ли животное клаустрофобией.
    /// </summary>
    /// <remarks>
    /// Животные с клаустрофобией не могут содержаться в приютах без открытой территории.
    /// При сериализации в XML будет использовано имя элемента "Claustrophobic".
    /// </remarks>
    [XmlElement("Claustrophobic")]
    public bool Claustrophobic { get; set; }
    
    /// <summary>
    /// Свойство, возвращающее имя типа животного для использования в сериализации.
    /// </summary>
    /// <remarks>
    /// Используется для определения типа животного при JSON и XML сериализации.
    /// </remarks>
    [JsonProperty("PetType")]
    [XmlElement("PetType")]
    public string PetType => this.GetType().Name;

    /// <summary>
    /// Расширенный конструктор, добавляющий параметр клаустрофобии.
    /// </summary>
    /// <param name="name">Имя животного (не может быть пустым)</param>
    /// <param name="age">Возраст животного (должен быть положительным)</param>
    /// <param name="weight">Вес животного (должен быть положительным)</param>
    /// <param name="claustrophobic">Флаг клаустрофобии</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если какой-либо из параметров имеет недопустимое значение.
    /// </exception>
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

    /// <summary>
    /// Переопределение метода ToString для предоставления удобочитаемого представления животного.
    /// </summary>
    /// <returns>Строковое представление объекта с его основными свойствами</returns>
    public override string ToString()
    {
        return $"Тип: {Type}, Имя: {Name}, Возраст: {Age}, Вес: {Weight}, " +
               $"Клаустрофобия: {(Claustrophobic ? "Да" : "Нет")}";
    }

    public string ClaustrophobicText => Claustrophobic ? "Да" : "Нет";
}