namespace PetShelter.Model.Core;

using System.Xml.Serialization;

[XmlType("Rabbit")]
public partial class Rabbit : Pet
{
    // Пустой конструктор для XML-сериализации
    public Rabbit() { }
    
    [XmlIgnore]
    public override Type Type => typeof(Rabbit);
    
    [XmlElement("EarType")]
    public string EarType { get; set; }
    
    [XmlElement("IsGiant")]
    public bool IsGiant { get; set; }

    public Rabbit(string name, int age, double weight, bool claustrophobic, string earType, bool isGiant)
        : base(name, age, weight, claustrophobic)
    {
        EarType = earType;
        IsGiant = isGiant;
    }
}

public partial class Rabbit
{
    public override string ToString()
    {
        return $"{base.ToString()}, Тип ушей: {EarType}, Гигант: {(IsGiant ? "Да" : "Нет")}";
    }
}