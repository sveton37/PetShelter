namespace PetShelter.Model.Core.Interfaces
{
    public interface IChangeable
    {
        // методы добавления и удаления животного из приюта
        Pet Add(Pet pet);
        Pet Remove(Pet pet);
    }
}
