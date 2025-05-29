namespace PetShelter.Model.Core;

public partial class Rabbit : Pet
{
    public string TypeOfEars { get; private set; }
    public bool IsGiant { get; private set; }

    public Rabbit(string name, int age, double weight, string typeOfEars, bool isGiant)
        : base(name, age, weight)
    {
        TypeOfEars = typeOfEars ?? throw new ArgumentNullException(nameof(typeOfEars));
        IsGiant = isGiant;
    }

    public override Type Type => typeof(Rabbit);
}

public partial class Rabbit : Pet
{
    public Rabbit(string name, int age, double weight, bool claustrophobic, string typeOfEars, bool isGiant) : base(
        name, age, weight, claustrophobic)
    {
        TypeOfEars = typeOfEars;
        IsGiant = isGiant;
    }
}