namespace PetShelter.Model.Core;

public partial class Dog : Pet
{
    public bool IsTrained { get; private set; }
    public string FoodType { get; private set; }

    public Dog(string name, int age, double weight, bool isTrained, string foodType)
        : base(name, age, weight)
    {
        IsTrained = isTrained;
        FoodType = foodType ?? throw new ArgumentNullException(nameof(foodType));
    }

    public override Type Type => typeof(Dog);
}

public partial class Dog : Pet
{
    public Dog(string name, int age, double weight, bool claustrophobic, bool isTrained, string foodType)
        : base(name, age, weight, claustrophobic)
    {
        IsTrained = isTrained;
        FoodType = foodType;
    }
}