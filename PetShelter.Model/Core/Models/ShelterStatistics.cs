namespace PetShelter.Model.Core.Models;

/// <summary>
/// Класс, представляющий статистические данные о приюте.
/// Используется для сбора и отображения статистической информации 
/// о животных в приюте в окне статистики.
/// </summary>
public class ShelterStatistics
{
    /// <summary>
    /// Общее количество животных в приюте.
    /// </summary>
    public int TotalPets { get; set; }
    
    /// <summary>
    /// Количество собак в приюте.
    /// </summary>
    public int Dogs { get; set; }
    
    /// <summary>
    /// Количество кошек в приюте.
    /// </summary>
    public int Cats { get; set; }
    
    /// <summary>
    /// Количество кроликов в приюте.
    /// </summary>
    public int Rabbits { get; set; }
    
    /// <summary>
    /// Количество животных с клаустрофобией в приюте.
    /// </summary>
    public int ClaustrophobicPets { get; set; }
    
    /// <summary>
    /// Средний возраст всех животных в приюте.
    /// </summary>
    public double AverageAge { get; set; }
    
    /// <summary>
    /// Средний вес всех животных в приюте.
    /// </summary>
    public double AverageWeight { get; set; }
    
    /// <summary>
    /// Количество свободных мест в приюте.
    /// Рассчитывается как разница между вместимостью приюта и количеством животных.
    /// </summary>
    public int AvailableSpace { get; set; }
    
    /// <summary>
    /// Процент заполненности приюта.
    /// Рассчитывается как отношение количества животных к вместимости приюта.
    /// </summary>
    public double OccupancyPercentage { get; set; }

}