namespace PetShelter.Model.Core.Interfaces
{
    public interface IFilter
    {
        IEnumerable<Pet> Filter(Type pet);
    }
}
