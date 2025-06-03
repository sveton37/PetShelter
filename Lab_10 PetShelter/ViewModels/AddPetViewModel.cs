using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Helper;
using PetShelter.Model.Core;

namespace Lab_10_PetShelter.ViewModels
{
    public class AddPetViewModel : BaseViewModel
    {
        private readonly Window _window;
        private readonly Shelter _shelter;
        
        // Основные свойства
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
                }
            }
        }

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

        // Специфичные свойства для разных типов животных
        public bool IsTrained
        {
            get => _isTrained;
            set => SetField(ref _isTrained, value);
        }

        public string FoodType
        {
            get => _foodType;
            set => SetField(ref _foodType, value);
        }

        public bool IsVaccinated
        {
            get => _isVaccinated;
            set => SetField(ref _isVaccinated, value);
        }

        public string AdoptionStatus
        {
            get => _adoptionStatus;
            set => SetField(ref _adoptionStatus, value);
        }

        public string EarType
        {
            get => _earType;
            set => SetField(ref _earType, value);
        }

        public bool IsGiant
        {
            get => _isGiant;
            set => SetField(ref _isGiant, value);
        }

        // Свойства видимости для специфичных полей
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

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}