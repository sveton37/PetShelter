namespace PetShelter.Model.Core.Models;

public class ShelterStatistics
{
    public int TotalPets { get; set; }
    public int Dogs { get; set; }
    public int Cats { get; set; }
    public int Rabbits { get; set; }
    public int ClaustrophobicPets { get; set; }
    public double AverageAge { get; set; }
    public double AverageWeight { get; set; }
    public int AvailableSpace { get; set; }
    public double OccupancyPercentage { get; set; }

}