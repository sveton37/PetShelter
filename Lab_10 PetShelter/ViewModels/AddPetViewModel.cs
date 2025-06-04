using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Helper;
using PetShelter.Model.Core;

namespace Lab_10_PetShelter.ViewModels;

/// <summary>
/// Модель представления для окна добавления нового животного в приют.
/// Обеспечивает функциональность создания различных типов животных
/// с учетом их специфических характеристик.
/// </summary>
public class AddPetViewModel : BaseViewModel
{
    /// <summary>
    /// Ссылка на окно, содержащее форму добавления животного.
    /// </summary>
    private readonly Window _window;
    
    /// <summary>
    /// Приют, в который будет добавлено новое животное.
    /// </summary>
    private readonly Shelter _shelter;

    // Основные свойства
    
    /// <summary>
    /// Выбранный тип животного (Dog, Cat, Rabbit).
    /// </summary>
    private Type _selectedPetType;
    
    /// <summary>
    /// Имя животного.
    /// </summary>
    private string _name;
    
    /// <summary>
    /// Возраст животного (в годах).
    /// </summary>
    private int _age;
    
    /// <summary>
    /// Вес животного (в килограммах).
    /// </summary>
    private double _weight;
    
    /// <summary>
    /// Флаг, указывающий на наличие клаустрофобии у животного.
    /// </summary>
    private bool _isClaustrophobic;

    // Специфичные свойства
    
    /// <summary>
    /// Флаг дрессировки (специфично для собак).
    /// </summary>
    private bool _isTrained;
    
    /// <summary>
    /// Тип питания (специфично для собак).
    /// </summary>
    private string _foodType;
    
    /// <summary>
    /// Флаг вакцинации (специфично для кошек).
    /// </summary>
    private bool _isVaccinated;
    
    /// <summary>
    /// Статус усыновления (специфично для кошек).
    /// </summary>
    private string _adoptionStatus;
    
    /// <summary>
    /// Тип ушей (специфично для кроликов).
    /// </summary>
    private string _earType;
    
    /// <summary>
    /// Флаг, указывающий, является ли кролик гигантским (специфично для кроликов).
    /// </summary>
    private bool _isGiant;

    /// <summary>
    /// Коллекция доступных типов животных для выбора.
    /// </summary>
    public ObservableCollection<Type> PetTypes { get; }
    
    /// <summary>
    /// Команда для добавления нового животного.
    /// </summary>
    public ICommand AddCommand { get; }
    
    /// <summary>
    /// Команда для отмены добавления животного и закрытия окна.
    /// </summary>
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Выбранный тип животного с обновлением видимости специфических полей.
    /// </summary>
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
            }
        }
    }

    /// <summary>
    /// Имя животного.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    /// <summary>
    /// Возраст животного (должен быть положительным).
    /// </summary>
    public int Age
    {
        get => _age;
        set => SetField(ref _age, value);
    }

    /// <summary>
    /// Вес животного (должен быть положительным).
    /// </summary>
    public double Weight
    {
        get => _weight;
        set => SetField(ref _weight, value);
    }

    /// <summary>
    /// Флаг, указывающий на наличие клаустрофобии у животного.
    /// </summary>
    public bool IsClaustrophobic
    {
        get => _isClaustrophobic;
        set => SetField(ref _isClaustrophobic, value);
    }

    // Специфичные свойства для разных типов животных
    
    /// <summary>
    /// Флаг дрессировки (для собак).
    /// </summary>
    public bool IsTrained
    {
        get => _isTrained;
        set => SetField(ref _isTrained, value);
    }

    /// <summary>
    /// Тип питания (для собак).
    /// </summary>
    public string FoodType
    {
        get => _foodType;
        set => SetField(ref _foodType, value);
    }

    /// <summary>
    /// Флаг вакцинации (для кошек).
    /// </summary>
    public bool IsVaccinated
    {
        get => _isVaccinated;
        set => SetField(ref _isVaccinated, value);
    }

    /// <summary>
    /// Статус усыновления (для кошек).
    /// </summary>
    public string AdoptionStatus
    {
        get => _adoptionStatus;
        set => SetField(ref _adoptionStatus, value);
    }

    /// <summary>
    /// Тип ушей (для кроликов).
    /// </summary>
    public string EarType
    {
        get => _earType;
        set => SetField(ref _earType, value);
    }

    /// <summary>
    /// Флаг, указывающий, является ли кролик гигантским (для кроликов).
    /// </summary>
    public bool IsGiant
    {
        get => _isGiant;
        set => SetField(ref _isGiant, value);
    }

    // Свойства видимости для специфичных полей
    
    /// <summary>
    /// Видимость специфических свойств для собак.
    /// </summary>
    public Visibility DogPropertiesVisibility =>
        SelectedPetType == typeof(Dog) ? Visibility.Visible : Visibility.Collapsed;

    
    /// <summary>
    /// Видимость специфических свойств для кошек.
    /// </summary>
    public Visibility CatPropertiesVisibility =>
        SelectedPetType == typeof(Cat) ? Visibility.Visible : Visibility.Collapsed;

    /// <summary>
    /// Видимость специфических свойств для кроликов.
    /// </summary>
    public Visibility RabbitPropertiesVisibility =>
        SelectedPetType == typeof(Rabbit) ? Visibility.Visible : Visibility.Collapsed;
    
    /// <summary>
    /// Конструктор AddPetViewModel.
    /// </summary>
    /// <param name="window">Окно, содержащее форму добавления животного</param>
    /// <param name="shelter">Приют, в который будет добавлено животное</param>
    public AddPetViewModel(Window window, Shelter shelter)
    {
        _window = window;
        _shelter = shelter;

        // Инициализация коллекции типов животных
        PetTypes = new ObservableCollection<Type>
        {
            typeof(Dog),
            typeof(Cat),
            typeof(Rabbit)
        };

        // Инициализация команд
        AddCommand = new RelayCommand(Add, CanAdd);
        CancelCommand = new RelayCommand(Cancel);
    }

    /// <summary>
    /// Проверяет, можно ли добавить животное (все обязательные поля заполнены).
    /// </summary>
    /// <returns>true, если все необходимые поля заполнены корректно</returns>
    private bool CanAdd()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               Age > 0 &&
               Weight > 0 &&
               SelectedPetType != null;
    }

    /// <summary>
    /// Создает новое животное выбранного типа и добавляет его в приют.
    /// </summary>
    private void Add()
    {
        try
        {
            // Создаем питомца нужного типа
            var pet = CreatePet();

            // Добавляем питомца в приют
            _shelter.AddPet(pet);

            // Закрываем окно с результатом true
            _window.DialogResult = true;
            _window.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при добавлении питомца: {ex.Message}",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Создает новый экземпляр животного выбранного типа с заданными свойствами.
    /// </summary>
    /// <returns>Созданный экземпляр животного</returns>
    /// <exception cref="ArgumentException">Выбрасывается при некорректных данных</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если тип животного не выбран</exception>
    private Pet CreatePet()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Имя питомца не может быть пустым");

        if (Age <= 0)
            throw new ArgumentException("Возраст должен быть больше 0");

        if (Weight <= 0)
            throw new ArgumentException("Вес должен быть больше 0");

        return SelectedPetType switch
        {
            Type t when t == typeof(Dog) => new Dog(
                Name,
                Age,
                Weight,
                IsClaustrophobic,
                IsTrained,
                FoodType ?? "Стандартный корм"),

            Type t when t == typeof(Cat) => new Cat(
                Name,
                Age,
                Weight,
                IsClaustrophobic,
                IsVaccinated,
                AdoptionStatus ?? "Не определен"),

            Type t when t == typeof(Rabbit) => new Rabbit(
                Name,
                Age,
                Weight,
                IsClaustrophobic,
                EarType ?? "Стандартные",
                IsGiant),

            _ => throw new InvalidOperationException("Выберите тип животного")
        };
    }
    
    /// <summary>
    /// Отменяет добавление животного и закрывает окно.
    /// </summary>
    private void Cancel()
    {
        _window.DialogResult = false;
        _window.Close();
    }
}