using System.Xml.Serialization;

namespace PetShelter.Model.Core;

/// <summary>
/// Класс, представляющий собаку в приюте.
/// Наследуется от базового класса Pet и добавляет специфические для собак свойства.
/// </summary>
/// <remarks>
/// Класс разделен на две части с помощью partial для разделения основных свойств
/// и дополнительной функциональности.
/// </remarks>
[XmlType("Dog")]
public partial class Dog : Pet
{
    /// <summary>
    /// Пустой конструктор, необходимый для XML-сериализации.
    /// </summary>
    public Dog() { }
    
    /// <summary>
    /// Переопределение абстрактного свойства Type из базового класса.
    /// Указывает конкретный тип этого животного.
    /// </summary>
    /// <remarks>
    /// Не сериализуется в XML благодаря атрибуту XmlIgnore.
    /// </remarks>
    [XmlIgnore]
    public override Type Type => typeof(Dog);
    
    /// <summary>
    /// Свойство, указывающее, прошла ли собака дрессировку.
    /// </summary>
    [XmlElement("IsTrained")]
    public bool IsTrained { get; set; }
    
    /// <summary>
    /// Тип питания собаки (например, "Сухой корм", "Влажный корм", "Натуральный").
    /// </summary>
    [XmlElement("FoodType")]
    public string FoodType { get; set; }

    /// <summary>
    /// Конструктор класса Dog с полным набором параметров.
    /// </summary>
    /// <param name="name">Имя собаки</param>
    /// <param name="age">Возраст собаки в годах</param>
    /// <param name="weight">Вес собаки в килограммах</param>
    /// <param name="claustrophobic">Флаг, указывающий, страдает ли собака клаустрофобией</param>
    /// <param name="isTrained">Флаг, указывающий, прошла ли собака дрессировку</param>
    /// <param name="foodType">Тип питания собаки</param>
    public Dog(string name, int age, double weight, bool claustrophobic, bool isTrained, string foodType)
        : base(name, age, weight, claustrophobic)
    {
        IsTrained = isTrained;
        FoodType = foodType;
    }
}

/// <summary>
/// Вторая часть частичного класса Dog, содержащая дополнительные методы.
/// </summary>
public partial class Dog
{
    /// <summary>
    /// Переопределение метода ToString для включения специфических для собаки свойств.
    /// </summary>
    /// <returns>Строковое представление собаки с ее основными и специфическими свойствами</returns>
    public override string ToString()
    {
        return $"{base.ToString()}, Дрессированная: {(IsTrained ? "Да" : "Нет")}, Тип питания: {FoodType}";
    }
}