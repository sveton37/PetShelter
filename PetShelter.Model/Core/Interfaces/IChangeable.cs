namespace PetShelter.Model.Core.Interfaces;

/// <summary>
/// Интерфейс, определяющий методы для добавления и удаления животных в приюте.
/// Расширяет функциональность приюта в продвинутой части задания.
/// </summary>
public interface IChangeable
{
    /// <summary>
    /// Добавляет животное в приют.
    /// </summary>
    /// <param name="pet">Животное для добавления</param>
    /// <returns>Добавленное животное</returns>
    /// <exception cref="ArgumentNullException">Если pet равен null</exception>
    /// <exception cref="InvalidOperationException">
    /// Если приют переполнен, животное не может быть помещено из-за клаустрофобии,
    /// или животное с такими параметрами уже существует в приюте
    /// </exception>
    Pet Add(Pet pet);
    
    /// <summary>
    /// Удаляет животное из приюта.
    /// </summary>
    /// <param name="pet">Животное для удаления</param>
    /// <returns>Удаленное животное</returns>
    /// <exception cref="ArgumentNullException">Если pet равен null</exception>
    /// <exception cref="InvalidOperationException">
    /// Если животное не найдено в приюте
    /// </exception>
    Pet Remove(Pet pet);
}