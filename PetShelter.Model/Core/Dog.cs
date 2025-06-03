using System.Xml.Serialization;

namespace PetShelter.Model.Core;

[XmlType("Dog")]
public partial class Dog : Pet
{
    // Пустой конструктор для XML-сериализации
    public Dog() { }
    
    [XmlIgnore]
    public override Type Type => typeof(Dog);
    
    [XmlElement("IsTrained")]
    public bool IsTrained { get; set; }
    
    [XmlElement("FoodType")]
    public string FoodType { get; set; }

    public Dog(string name, int age, double weight, bool claustrophobic, bool isTrained, string foodType)
        : base(name, age, weight, claustrophobic)
    {
        IsTrained = isTrained;
        FoodType = foodType;
    }
}

public partial class Dog
{
    public override string ToString()
    {
        return $"{base.ToString()}, Дрессированная: {(IsTrained ? "Да" : "Нет")}, Тип питания: {FoodType}";
    }
}