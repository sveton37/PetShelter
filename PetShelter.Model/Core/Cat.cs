using System.Xml.Serialization;

namespace PetShelter.Model.Core;

/// <summary>
/// Класс, представляющий кошку в приюте.
/// Наследуется от базового класса Pet и добавляет специфические для кошек свойства.
/// </summary>
/// <remarks>
/// Класс разделен на две части с помощью partial для разделения основных свойств
/// и дополнительной функциональности.
/// </remarks>
[XmlType("Cat")]
public partial class Cat : Pet
{
    /// <summary>
    /// Пустой конструктор, необходимый для XML-сериализации.
    /// </summary>
    public Cat() { }
    
    /// <summary>
    /// Переопределение абстрактного свойства Type из базового класса.
    /// Указывает конкретный тип этого животного.
    /// </summary>
    /// <remarks>
    /// Не сериализуется в XML благодаря атрибуту XmlIgnore.
    /// </remarks>
    [XmlIgnore]
    public override Type Type => typeof(Cat);
    
    /// <summary>
    /// Свойство, указывающее, вакцинирована ли кошка.
    /// </summary>
    [XmlElement("IsVaccinated")]
    public bool IsVaccinated { get; set; }
    
    /// <summary>
    /// Статус усыновления кошки (например, "Доступна", "В процессе усыновления", "Усыновлена").
    /// </summary>
    [XmlElement("AdoptionStatus")]
    public string AdoptionStatus { get; set; }

    /// <summary>
    /// Конструктор класса Cat с полным набором параметров.
    /// </summary>
    /// <param name="name">Имя кошки</param>
    /// <param name="age">Возраст кошки в годах</param>
    /// <param name="weight">Вес кошки в килограммах</param>
    /// <param name="claustrophobic">Флаг, указывающий, страдает ли кошка клаустрофобией</param>
    /// <param name="isVaccinated">Флаг, указывающий, вакцинирована ли кошка</param>
    /// <param name="adoptionStatus">Статус усыновления кошки</param>
    public Cat(string name, int age, double weight, bool claustrophobic, bool isVaccinated, string adoptionStatus)
        : base(name, age, weight, claustrophobic)
    {
        IsVaccinated = isVaccinated;
        AdoptionStatus = adoptionStatus;
    }
}

/// <summary>
/// Вторая часть частичного класса Cat, содержащая дополнительные методы.
/// </summary>
public partial class Cat
{
    /// <summary>
    /// Переопределение метода ToString для включения специфических для кошки свойств.
    /// </summary>
    /// <returns>Строковое представление кошки с ее основными и специфическими свойствами</returns>
    public override string ToString()
    {
        return $"{base.ToString()}, Вакцинированная: {(IsVaccinated ? "Да" : "Нет")}, Статус: {AdoptionStatus}";
    }
}