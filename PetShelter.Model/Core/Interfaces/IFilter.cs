namespace PetShelter.Model.Core.Interfaces;

/// <summary>
/// Интерфейс, определяющий методы для фильтрации объектов по типу.
/// Используется для фильтрации животных в приюте.
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Фильтрует коллекцию объектов по указанному типу.
    /// </summary>
    /// <param name="pet">Тип объекта для фильтрации</param>
    /// <returns>Коллекция объектов, соответствующих указанному типу</returns>
    IEnumerable<Pet> Filter(Type pet);
}