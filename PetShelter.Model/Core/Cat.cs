using System.Xml.Serialization;

namespace PetShelter.Model.Core;

[XmlType("Cat")]
public partial class Cat : Pet
{
    // Пустой конструктор для XML-сериализации
    public Cat() { }
    
    [XmlIgnore]
    public override Type Type => typeof(Cat);
    
    [XmlElement("IsVaccinated")]
    public bool IsVaccinated { get; set; }
    
    [XmlElement("AdoptionStatus")]
    public string AdoptionStatus { get; set; }

    public Cat(string name, int age, double weight, bool claustrophobic, bool isVaccinated, string adoptionStatus)
        : base(name, age, weight, claustrophobic)
    {
        IsVaccinated = isVaccinated;
        AdoptionStatus = adoptionStatus;
    }
}

public partial class Cat
{
    public override string ToString()
    {
        return $"{base.ToString()}, Вакцинированная: {(IsVaccinated ? "Да" : "Нет")}, Статус: {AdoptionStatus}";
    }
}