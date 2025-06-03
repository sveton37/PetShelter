namespace PetShelter.Model.Core.Interfaces
{
     public interface ICountable
    {
        int Count();
        int Count(Type pet);
        int Percentage(Type pet);
    }
}
