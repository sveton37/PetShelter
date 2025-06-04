namespace PetShelter.Model.Core;

using System.Xml.Serialization;

/// <summary>
/// Класс, представляющий кролика в приюте.
/// Наследуется от базового класса Pet и добавляет специфические для кроликов свойства.
/// </summary>
/// <remarks>
/// Класс разделен на две части с помощью partial для разделения основных свойств
/// и дополнительной функциональности.
/// </remarks>
[XmlType("Rabbit")]
public partial class Rabbit : Pet
{
    /// <summary>
    /// Пустой конструктор, необходимый для XML-сериализации.
    /// </summary>
    public Rabbit() { }
    
    /// <summary>
    /// Переопределение абстрактного свойства Type из базового класса.
    /// Указывает конкретный тип этого животного.
    /// </summary>
    /// <remarks>
    /// Не сериализуется в XML благодаря атрибуту XmlIgnore.
    /// </remarks>
    [XmlIgnore]
    public override Type Type => typeof(Rabbit);
    
    /// <summary>
    /// Тип ушей кролика (например, "Стоячие", "Висячие", "Полустоячие").
    /// </summary>
    [XmlElement("EarType")]
    public string EarType { get; set; }
    
    /// <summary>
    /// Флаг, указывающий, является ли кролик гигантской породой.
    /// </summary>
    [XmlElement("IsGiant")]
    public bool IsGiant { get; set; }

    /// <summary>
    /// Конструктор класса Rabbit с полным набором параметров.
    /// </summary>
    /// <param name="name">Имя кролика</param>
    /// <param name="age">Возраст кролика в годах</param>
    /// <param name="weight">Вес кролика в килограммах</param>
    /// <param name="claustrophobic">Флаг, указывающий, страдает ли кролик клаустрофобией</param>
    /// <param name="earType">Тип ушей кролика</param>
    /// <param name="isGiant">Флаг, указывающий, является ли кролик гигантской породой</param>
    public Rabbit(string name, int age, double weight, bool claustrophobic, string earType, bool isGiant)
        : base(name, age, weight, claustrophobic)
    {
        EarType = earType;
        IsGiant = isGiant;
    }
}

/// <summary>
/// Вторая часть частичного класса Rabbit, содержащая дополнительные методы.
/// </summary>
public partial class Rabbit
{
    /// <summary>
    /// Переопределение метода ToString для включения специфических для кролика свойств.
    /// </summary>
    /// <returns>Строковое представление кролика с его основными и специфическими свойствами</returns>
    public override string ToString()
    {
        return $"{base.ToString()}, Тип ушей: {EarType}, Гигант: {(IsGiant ? "Да" : "Нет")}";
    }
}