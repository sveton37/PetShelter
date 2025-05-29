using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Views;
using PetShelter.Model.Core;
using PetShelter.Model.Data;

namespace Lab_10_PetShelter.ViewModels;

public class MainViewModel : BaseViewModel
{
    private List<Shelter> _sheltersList;
    private ObservableCollection<Type> _petTypes;
    private Shelter _selectedShelter;
    private Type _selectedPetType;
    private bool _showOpenYardOnly;
    private string _selectedFormat;
    private readonly JsonSerializer _jsonSerializer;
    private readonly XmlSerializer _xmlSerializer;
    private int _selectionCounter = 0;
    private ObservableCollection<Shelter> _shelters;
    
    public ObservableCollection<Shelter> Shelters
    {
        get => _shelters;
        set => SetField(ref _shelters, value);
    }
    
    public ObservableCollection<Type> PetTypes
    {
        get => _petTypes;
        set => SetField(ref _petTypes, value);
    }

    public Shelter SelectedShelter
    {
        get => _selectedShelter;
        set => SetField(ref _selectedShelter, value);
    }

    public Type SelectedPetType
    {
        get => _selectedPetType;
        set => SetField(ref _selectedPetType, value);
    }

    public bool ShowOpenYardOnly
    {
        get => _showOpenYardOnly;
        set => SetField(ref _showOpenYardOnly, value);
    }

    public string SelectedFormat
    {
        get => _selectedFormat;
        set
        {
            if (SetField(ref _selectedFormat, value))
            {
                ConvertReports();
            }
        }
    }

    public ObservableCollection<string> SerializationFormats { get; }

    public ICommand ShowPetsCommand { get; }

    public ICommand ShowStatisticsCommand { get; }

    public MainViewModel()
    {

        _jsonSerializer = new JsonSerializer();
        _xmlSerializer = new XmlSerializer();
        SerializationFormats = new ObservableCollection<string> { "JSON", "XML" };
        _selectedFormat = "JSON";

        InitializeCollections();
        LoadData();

        ShowPetsCommand = new RelayCommand(ShowPets, CanShowPets);
        ShowStatisticsCommand = new RelayCommand(ShowStatistics, () => SelectedShelter != null);
    }

    private void InitializeCollections()
    {
        Shelters = new ObservableCollection<Shelter>();
        PetTypes = new ObservableCollection<Type>
        {
            typeof(Dog),
            typeof(Cat),
            typeof(Rabbit)
        };
    }

    private void LoadData()
    {
        try
        {
            // Загрузка данных при старте приложения
            _sheltersList = DataInitializer.LoadData();
            
            if (_sheltersList == null || !_sheltersList.Any())
            {
                MessageBox.Show("Список приютов пуст", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Преобразуем список в ObservableCollection
            Shelters = new ObservableCollection<Shelter>(_sheltersList);
            // Выбираем первый приют по умолчанию, если список не пустой
            if (Shelters.Any())
            {
                SelectedShelter = Shelters.FirstOrDefault();
            }
        }
        catch (FileNotFoundException ex)
        {
            MessageBox.Show($"Файл с данными не найден: {ex.Message}", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            try
            {
                // Если файл не найден, создаем начальные данные
                var initialShelters = DataInitializer.CreateInitialData();
                foreach (var shelter in initialShelters)
                {
                    Shelters.Add(shelter);
                }
                
                Shelters = new ObservableCollection<Shelter>(initialShelters);
                if (Shelters.Any())
                {
                    SelectedShelter = Shelters.FirstOrDefault();
                }

            }
            catch (Exception initEx)
            {
                MessageBox.Show($"Ошибка при создании начальных данных: {initEx.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanShowPets()
    {
        return true; // Можно добавить дополнительные условия
    }

    private void ShowPets()
    {
        // Получаем отфильтрованных питомцев
        var filteredPets = SelectedShelter?.Filter(SelectedPetType ?? typeof(Pet));
        if (filteredPets != null && filteredPets.Any())
        {
            // Сохраняем результаты подборки
            SaveResultsToFile(filteredPets);

            // Показываем окно с питомцами
            var petsViewModel = new PetsViewModel(SelectedShelter, SelectedPetType, ShowOpenYardOnly);
            var petsView = new PetsView { DataContext = petsViewModel };
            petsView.Show();
        }
        else
        {
            MessageBox.Show("Нет питомцев, соответствующих выбранным критериям", 
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public void ConvertReports()
    {
        var sourceFormat = SelectedFormat == "JSON" ? "xml" : "json";
        var files = Directory.GetFiles(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"),
            $"*.{sourceFormat}");

        foreach (var file in files)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var data = SelectedFormat == "JSON" 
                    ? _xmlSerializer.Deserialize<object>(fileName)
                    : _jsonSerializer.Deserialize<object>(fileName);

                if (SelectedFormat == "JSON")
                    _jsonSerializer.Serialize(fileName, data);
                else
                    _xmlSerializer.Serialize(fileName, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка конвертации файла {file}: {ex.Message}");
            }
        }
    }

    private void ShowStatistics()
    {
        var statisticsWindow = new StatisticsView();
        var viewModel = new StatisticsViewModel(SelectedShelter);
        statisticsWindow.DataContext = viewModel;
        statisticsWindow.Owner = Application.Current.MainWindow;
        statisticsWindow.ShowDialog();
    }

    private void SaveResultsToFile(IEnumerable<Pet> pets)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
        var fileName = $"Подборка_{++_selectionCounter}_от_{timestamp}";
        
        try
        {
            if (SelectedFormat == "JSON")
                _jsonSerializer.Serialize(fileName, pets);
            else
                _xmlSerializer.Serialize(fileName, pets);

            MessageBox.Show($"Результаты сохранены в файл {fileName}", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}