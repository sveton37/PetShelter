using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.ViewModels;
using PetShelter.Model.Core;

namespace Lab_10_PetShelter.Views;

public class AddPetViewModel : BaseViewModel
{
    private readonly Window _window;
    private readonly Shelter _shelter;
    private Type _selectedPetType;
    private string _name;
    private int _age;
    private double _weight;
    private bool _isClaustrophobic;

    // Специфичные свойства
    private bool _isTrained;
    private string _foodType;
    private bool _isVaccinated;
    private string _adoptionStatus;
    private string _earType;
    private bool _isGiant;

    public ObservableCollection<Type> PetTypes { get; }
    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }

    public Type SelectedPetType
    {
        get => _selectedPetType;
        set
        {
            if (SetField(ref _selectedPetType, value))
            {
                OnPropertyChanged(nameof(DogPropertiesVisibility));
                OnPropertyChanged(nameof(CatPropertiesVisibility));
                OnPropertyChanged(nameof(RabbitPropertiesVisibility));
                OnPropertyChanged(nameof(SpecificPropertiesVisibility));
            }
        }
    }

    // Остальные свойства
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public int Age
    {
        get => _age;
        set => SetField(ref _age, value);
    }

    public double Weight
    {
        get => _weight;
        set => SetField(ref _weight, value);
    }

    public bool IsClaustrophobic
    {
        get => _isClaustrophobic;
        set => SetField(ref _isClaustrophobic, value);
    }

    // Видимость специфичных свойств
    public Visibility SpecificPropertiesVisibility =>
        SelectedPetType != null ? Visibility.Visible : Visibility.Collapsed;

    public Visibility DogPropertiesVisibility =>
        SelectedPetType == typeof(Dog) ? Visibility.Visible : Visibility.Collapsed;

    public Visibility CatPropertiesVisibility =>
        SelectedPetType == typeof(Cat) ? Visibility.Visible : Visibility.Collapsed;

    public Visibility RabbitPropertiesVisibility =>
        SelectedPetType == typeof(Rabbit) ? Visibility.Visible : Visibility.Collapsed;

    public AddPetViewModel(Window window, Shelter shelter)
    {
        _window = window;
        _shelter = shelter;
        PetTypes = new ObservableCollection<Type> { typeof(Dog), typeof(Cat), typeof(Rabbit) };

        AddCommand = new RelayCommand(Add, CanAdd);
        CancelCommand = new RelayCommand(Cancel);
    }

    private bool CanAdd()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               Age > 0 &&
               Weight > 0 &&
               SelectedPetType != null;
    }

    private void Add()
    {
        try
        {
            Pet newPet = CreatePet();
            _shelter.Add(newPet);
            _window.DialogResult = true;
            _window.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при добавлении питомца: {ex.Message}");
        }
    }

    private Pet CreatePet()
    {
        return SelectedPetType switch
        {
            Type t when t == typeof(Dog) => new Dog(Name, Age, Weight, IsClaustrophobic, _isTrained, _foodType),
            Type t when t == typeof(Cat) => new Cat(Name, Age, Weight, IsClaustrophobic, _isVaccinated,
                _adoptionStatus),
            Type t when t == typeof(Rabbit) => new Rabbit(Name, Age, Weight, IsClaustrophobic, _earType, _isGiant),
            _ => throw new InvalidOperationException("Неизвестный тип животного")
        };
    }

    private void Cancel()
    {
        _window.DialogResult = false;
        _window.Close();
    }
}