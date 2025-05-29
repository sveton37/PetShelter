namespace PetShelter.Model.Core;

public partial class Cat : Pet
{
    public bool IsVaccinated { get; private set; }
    public string IsAdopted { get; private set; }

    public Cat(string name, int age, double weight, bool isVaccinated, string isAdopted)
        : base(name, age, weight)
    {
        IsVaccinated = isVaccinated;
        IsAdopted = isAdopted ?? throw new ArgumentNullException(nameof(isAdopted));
    }

    public override Type Type => typeof(Cat);
}

public partial class Cat : Pet
{
    public Cat(string name, int age, double weight, bool claustrophobic, bool isVacinated, string isAdopted) : base(
        name, age, weight, claustrophobic)
    {
        IsVaccinated = isVacinated;
        IsAdopted = isAdopted;
    }
}