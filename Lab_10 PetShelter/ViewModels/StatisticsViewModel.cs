using System.Windows;
using System.Windows.Media;
using PetShelter.Model.Core;
using PetShelter.Model.Core.Models;

namespace Lab_10_PetShelter.ViewModels;

public class StatisticsViewModel : BaseViewModel
{
    private readonly Shelter _shelter;
    private ShelterStatistics _statistics;

    public string ShelterName => _shelter.Name;

    public string TotalPetsInfo => $"Всего питомцев: {_statistics.TotalPets}";
    public string AvailableSpaceInfo => $"Свободных мест: {_statistics.AvailableSpace}";
    public string OccupancyInfo => $"Заполненность: {_statistics.OccupancyPercentage:F1}%";
    public string DogsInfo => $"Собак: {_statistics.Dogs} ({GetPercentage(_statistics.Dogs)}%)";
    public string CatsInfo => $"Кошек: {_statistics.Cats} ({GetPercentage(_statistics.Cats)}%)";
    public string RabbitsInfo => $"Кроликов: {_statistics.Rabbits} ({GetPercentage(_statistics.Rabbits)}%)";
    public string AverageAgeInfo => $"Средний возраст: {_statistics.AverageAge:F1} лет";
    public string AverageWeightInfo => $"Средний вес: {_statistics.AverageWeight:F1} кг";

    public string ClaustrophobicInfo =>
        $"Клаустрофобов: {_statistics.ClaustrophobicPets} ({GetPercentage(_statistics.ClaustrophobicPets)}%)";

    // Свойства для диаграммы
    public Geometry DogsSegment { get; private set; }
    public Geometry CatsSegment { get; private set; }
    public Geometry RabbitsSegment { get; private set; }

    public Brush DogsSegmentBrush => new SolidColorBrush(Colors.Blue);
    public Brush CatsSegmentBrush => new SolidColorBrush(Colors.Orange);
    public Brush RabbitsSegmentBrush => new SolidColorBrush(Colors.Green);

    public StatisticsViewModel(Shelter shelter)
    {
        _shelter = shelter;
        CalculateStatistics();
        CreatePieChart();
    }

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
    
    private double GetPercentage(int count)
    {
        return _statistics.TotalPets == 0 ? 0 : (double)count / _statistics.TotalPets * 100;
    }

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