using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Views;
using PetShelter.Model.Core;

namespace Lab_10_PetShelter.ViewModels;

public class PetsViewModel : BaseViewModel
{
    private readonly Shelter _shelter;
    private ObservableCollection<Pet> _filteredPets;
    private Pet _selectedPet;
    private readonly Type _selectedPetType;
    private readonly bool _showOpenYardOnly;

    public ObservableCollection<Pet> FilteredPets
    {
        get => _filteredPets;
        set => SetField(ref _filteredPets, value);
    }

    public Pet SelectedPet
    {
        get => _selectedPet;
        set => SetField(ref _selectedPet, value);
    }

    public ICommand AddPetCommand { get; }
    public ICommand RemovePetCommand { get; }

    public PetsViewModel(Shelter shelter, Type petType, bool showOpenYardOnly)
    {
        _shelter = shelter;
        _showOpenYardOnly = showOpenYardOnly;
        _selectedPetType = petType;
        AddPetCommand = new RelayCommand(AddPet, CanAddPet);
        RemovePetCommand = new RelayCommand(RemovePet, CanRemovePet);

        FilterPets(petType, showOpenYardOnly);
    }

    private void FilterPets(Type petType, bool showOpenYardOnly)
    {
        // Получаем всех питомцев
        var pets = petType == null ? 
            _shelter.Filter(typeof(Pet)) : 
            _shelter.Filter(petType);

        if (showOpenYardOnly)
        {
            pets = pets.Where(p => !p.Claustrophobic || _shelter.HasOpenYard);
        }

        FilteredPets = new ObservableCollection<Pet>(pets);
    }

    private bool CanAddPet() => _shelter != null;
    private bool CanRemovePet() => SelectedPet != null;

    private void AddPet()
    {
        var addPetWindow = new AddPetView();
        var viewModel = new AddPetViewModel(addPetWindow, _shelter);
        addPetWindow.DataContext = viewModel;
        addPetWindow.Owner = Application.Current.MainWindow;

        if (addPetWindow.ShowDialog() == true)
        {
            FilterPets(_selectedPetType, _showOpenYardOnly);
        }
    }

    private void RemovePet()
    {
        if (SelectedPet != null)
        {
            _shelter.Remove(SelectedPet);
            FilteredPets.Remove(SelectedPet);
        }
    }
}