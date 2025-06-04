using System.Windows;
using System.Windows.Media;
using PetShelter.Model.Core;
using PetShelter.Model.Core.Models;

namespace Lab_10_PetShelter.ViewModels;

/// <summary>
/// Модель представления для окна статистики приюта.
/// Обеспечивает функциональность отображения различных статистических данных 
/// и построения круговой диаграммы распределения типов животных.
/// </summary>
public class StatisticsViewModel : BaseViewModel
{
    /// <summary>
    /// Приют, для которого отображается статистика.
    /// </summary>
    private readonly Shelter _shelter;
    
    /// <summary>
    /// Объект со статистическими данными приюта.
    /// </summary>
    private ShelterStatistics _statistics;

    /// <summary>
    /// Название приюта.
    /// </summary>
    public string ShelterName => _shelter.Name;

    /// <summary>
    /// Строка с информацией об общем количестве животных.
    /// </summary>
    public string TotalPetsInfo => $"Всего питомцев: {_statistics.TotalPets}";
    
    /// <summary>
    /// Строка с информацией о количестве свободных мест.
    /// </summary>
    public string AvailableSpaceInfo => $"Свободных мест: {_statistics.AvailableSpace}";
    
    /// <summary>
    /// Строка с информацией о процентной заполненности приюта.
    /// </summary>
    public string OccupancyInfo => $"Заполненность: {_statistics.OccupancyPercentage:F1}%";
    
    /// <summary>
    /// Строка с информацией о количестве собак.
    /// </summary>
    public string DogsInfo => $"Собак: {_statistics.Dogs} ({GetPercentage(_statistics.Dogs)}%)";
    
    /// <summary>
    /// Строка с информацией о количестве кошек.
    /// </summary>
    public string CatsInfo => $"Кошек: {_statistics.Cats} ({GetPercentage(_statistics.Cats)}%)";
    
    /// <summary>
    /// Строка с информацией о количестве кроликов.
    /// </summary>
    public string RabbitsInfo => $"Кроликов: {_statistics.Rabbits} ({GetPercentage(_statistics.Rabbits)}%)";
    
    /// <summary>
    /// Строка с информацией о среднем возрасте животных.
    /// </summary>
    public string AverageAgeInfo => $"Средний возраст: {_statistics.AverageAge:F1} лет";
    
    /// <summary>
    /// Строка с информацией о среднем весе животных.
    /// </summary>
    public string AverageWeightInfo => $"Средний вес: {_statistics.AverageWeight:F1} кг";

    /// <summary>
    /// Строка с информацией о количестве животных с клаустрофобией.
    /// </summary>
    public string ClaustrophobicInfo =>
        $"Клаустрофобов: {_statistics.ClaustrophobicPets} ({GetPercentage(_statistics.ClaustrophobicPets)}%)";

    // Свойства для диаграммы
    
    /// <summary>
    /// Геометрия сегмента круговой диаграммы для собак.
    /// </summary>
    public Geometry DogsSegment { get; private set; }
    
    /// <summary>
    /// Геометрия сегмента круговой диаграммы для кошек.
    /// </summary>
    public Geometry CatsSegment { get; private set; }
    
    /// <summary>
    /// Геометрия сегмента круговой диаграммы для кроликов.
    /// </summary>
    public Geometry RabbitsSegment { get; private set; }

    /// <summary>
    /// Кисть для заполнения сегмента собак (синий цвет).
    /// </summary>
    public Brush DogsSegmentBrush => new SolidColorBrush(Colors.Blue);
    
    /// <summary>
    /// Кисть для заполнения сегмента кошек (оранжевый цвет).
    /// </summary>
    public Brush CatsSegmentBrush => new SolidColorBrush(Colors.Orange);
    
    /// <summary>
    /// Кисть для заполнения сегмента кроликов (зеленый цвет).
    /// </summary>
    public Brush RabbitsSegmentBrush => new SolidColorBrush(Colors.Green);

    /// <summary>
    /// Конструктор StatisticsViewModel.
    /// </summary>
    /// <param name="shelter">Приют, для которого нужно отобразить статистику</param>
    public StatisticsViewModel(Shelter shelter)
    {
        _shelter = shelter;
        CalculateStatistics();
        CreatePieChart();
    }

    /// <summary>
    /// Рассчитывает статистические данные приюта.
    /// </summary>
    private void CalculateStatistics()
    {
        var allPets = _shelter.Filter(typeof(Pet)).ToList();
        
        if (allPets.Count == 0)
        {
            _statistics = new ShelterStatistics
            {
                TotalPets           = 0,
                Dogs                = 0,
                Cats                = 0,
                Rabbits             = 0,
                ClaustrophobicPets  = 0,
                AverageAge          = 0,
                AverageWeight       = 0,
                AvailableSpace      = _shelter.Capacity,
               OccupancyPercentage = 0
            };
            return;
        }
        
        _statistics = new ShelterStatistics
        {
            TotalPets = _shelter.Count(),
            Dogs = _shelter.Count(typeof(Dog)),
            Cats = _shelter.Count(typeof(Cat)),
            Rabbits = _shelter.Count(typeof(Rabbit)),
            ClaustrophobicPets = allPets.Count(p => p.Claustrophobic),
            AverageAge = allPets.Average(p => p.Age),
            AverageWeight = allPets.Average(p => p.Weight),
            AvailableSpace = _shelter.Capacity - _shelter.Count(),
            OccupancyPercentage = (double)_shelter.Count() / _shelter.Capacity * 100
        };
    }
    
    /// <summary>
    /// Вычисляет процентное соотношение животных указанного типа к общему количеству.
    /// </summary>
    /// <param name="count">Количество животных определенного типа</param>
    /// <returns>Процентное соотношение (0 если общее количество равно 0)</returns>
    private double GetPercentage(int count)
    {
        return _statistics.TotalPets == 0 ? 0 : (double)count / _statistics.TotalPets * 100;
    }

    /// <summary>
    /// Создает круговую диаграмму для визуального отображения статистики.
    /// </summary>
    private void CreatePieChart()
    {
        double total = _statistics.TotalPets;
        if (total == 0) return;

        double startAngle = 0;
        double centerX = 100;
        double centerY = 100;
        double radius = 90;

        // Создаем сегменты для каждого типа животных
        DogsSegment = CreatePieSegment(centerX, centerY, radius, startAngle,
            startAngle + 360 * _statistics.Dogs / total);

        startAngle += 360 * _statistics.Dogs / total;
        CatsSegment = CreatePieSegment(centerX, centerY, radius, startAngle,
            startAngle + 360 * _statistics.Cats / total);

        startAngle += 360 * _statistics.Cats / total;
        RabbitsSegment = CreatePieSegment(centerX, centerY, radius, startAngle,
            startAngle + 360 * _statistics.Rabbits / total);
    }

    /// <summary>
    /// Создает геометрию сегмента круговой диаграммы.
    /// </summary>
    /// <param name="centerX">X-координата центра круга</param>
    /// <param name="centerY">Y-координата центра круга</param>
    /// <param name="radius">Радиус круга</param>
    /// <param name="startAngle">Начальный угол сегмента</param>
    /// <param name="endAngle">Конечный угол сегмента</param>
    /// <returns>Объект геометрии для отображения сегмента</returns>
    private Geometry CreatePieSegment(double centerX, double centerY, double radius,
        double startAngle, double endAngle)
    {
        double startRadians = startAngle * Math.PI / 180;
        double endRadians = endAngle * Math.PI / 180;

        double x1 = centerX + radius * Math.Cos(startRadians);
        double y1 = centerY + radius * Math.Sin(startRadians);
        double x2 = centerX + radius * Math.Cos(endRadians);
        double y2 = centerY + radius * Math.Sin(endRadians);

        bool isLargeArc = endAngle - startAngle > 180;

        var pathFigure = new PathFigure(
            new Point(centerX, centerY),
            new PathSegment[]
            {
                new LineSegment(new Point(x1, y1), true),
                new ArcSegment(
                    new Point(x2, y2),
                    new Size(radius, radius),
                    0,
                    isLargeArc,
                    SweepDirection.Clockwise,
                    true),
                new LineSegment(new Point(centerX, centerY), true)
            },
            true);

        return new PathGeometry(new[] { pathFigure });
    }
}