namespace PetShelter.Model.Core.Interfaces;

/// <summary>
/// Интерфейс, определяющий методы для подсчета объектов разных типов.
/// Используется для подсчета количества животных в приюте.
/// </summary>
public interface ICountable
{
    /// <summary>
    /// Возвращает общее количество объектов.
    /// </summary>
    /// <returns>Количество объектов</returns>
    int Count();
    
    /// <summary>
    /// Возвращает количество объектов указанного типа.
    /// </summary>
    /// <param name="pet">Тип объекта для подсчета</param>
    /// <returns>Количество объектов указанного типа</returns>
    int Count(Type pet);
    
    /// <summary>
    /// Вычисляет процентное соотношение объектов указанного типа к общему количеству.
    /// </summary>
    /// <param name="pet">Тип объекта для расчета процента</param>
    /// <returns>Процент объектов указанного типа от общего количества</returns>
    int Percentage(Type pet);
}