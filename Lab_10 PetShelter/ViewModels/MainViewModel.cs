using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Helper;
using Lab_10_PetShelter.Views;
using PetShelter.Model.Core;
using PetShelter.Model.Data;

namespace Lab_10_PetShelter.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private List<Shelter> _sheltersList;

        private ObservableCollection<Shelter> _shelters;
        public ObservableCollection<Shelter> Shelters
        {
            get => _shelters;
            set => SetField(ref _shelters, value);
        }

        private ObservableCollection<Type> _petTypes;
        public ObservableCollection<Type> PetTypes
        {
            get => _petTypes;
            set => SetField(ref _petTypes, value);
        }

        private Shelter _selectedShelter;
        public Shelter SelectedShelter
        {
            get => _selectedShelter;
            set => SetField(ref _selectedShelter, value);
        }

        private Type _selectedPetType;
        public Type SelectedPetType
        {
            get => _selectedPetType;
            set
            {
                if (SetField(ref _selectedPetType, value))
                    UpdateSheltersFilter();
            }
        }

        private bool _showOpenYardOnly;
        public bool ShowOpenYardOnly
        {
            get => _showOpenYardOnly;
            set
            {
                if (SetField(ref _showOpenYardOnly, value))
                    UpdateSheltersFilter();
            }
        }
        
        private readonly SerializerBase _jsonSerializer;
        private readonly SerializerBase _xmlSerializer;

        public ICommand ShowPetsCommand { get; }
        public ICommand ShowStatisticsCommand { get; }
        
        private ReportFormat _selectedReportFormat = ReportFormat.Json;
        private static int _reportCounter = 0;

        public ObservableCollection<ReportFormat> ReportFormats { get; } = new ObservableCollection<ReportFormat>
        {
            ReportFormat.Json,
            ReportFormat.Xml
        };

        public ReportFormat SelectedReportFormat
        {
            get => _selectedReportFormat;
            set
            {
                if (_selectedReportFormat != value)
                {
                    _selectedReportFormat = value;
                    OnPropertyChanged(nameof(SelectedReportFormat));
                }
            }
        }

        // Команда для конвертации отчетов
        public ICommand ConvertReportsCommand { get; }
        
        public MainViewModel()
        {
            _jsonSerializer = new JsonSerializer();
            _xmlSerializer = new XmlSerializer();

            InitializeCollections();
            LoadData();

            ShowPetsCommand = new RelayCommand(ShowPets, CanShowPets);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics, () => SelectedShelter != null);
            ConvertReportsCommand = new RelayCommand(ConvertReportsBasedOnSelected);
        }

        // В методе InitializeCollections
        public void InitializeCollections()
        {
            // 1) Инициализируем пустую коллекцию приютов (заполнится в LoadData)
            Shelters = new ObservableCollection<Shelter>();

            // 2) Наполняем список типов только реальными классами (без typeof(Pet))
            PetTypes = new ObservableCollection<Type>
            {
                typeof(Dog),
                typeof(Cat),
                typeof(Rabbit)
            };
            
            // Для отладки
            Console.WriteLine("Инициализированы типы животных:");
            foreach (var type in PetTypes)
            {
                Console.WriteLine($"  Тип: {type.Name}, FullName: {type.FullName}, Assembly: {type.Assembly.GetName().Name}");
            }
        }

        // метод для определения формата конвертации
        private void ConvertReportsBasedOnSelected()
        {
            // Определяем форматы для конвертации
            ReportFormat fromFormat, toFormat;
            
            // Конвертируем из текущего формата в противоположный
            if (SelectedReportFormat == ReportFormat.Json)
            {
                fromFormat = ReportFormat.Json;
                toFormat = ReportFormat.Xml;
            }
            else
            {
                fromFormat = ReportFormat.Xml;
                toFormat = ReportFormat.Json;
            }
            
            // Выполняем конвертацию
            ConvertReports(fromFormat, toFormat);
        }

        private void LoadData()
        {
            try
            {
                // Сразу создаём «живые» данные вместе с питомцами
                _sheltersList = DataInitializer.CreateInitialData();

                Shelters = new ObservableCollection<Shelter>(_sheltersList);
                SelectedShelter = Shelters.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось создать начальные данные: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSheltersFilter()
        {
            if (_sheltersList == null)
                return;

            Console.WriteLine($"Фильтрация приютов. Выбранный тип: {SelectedPetType?.Name ?? "все"}");
            
            // 1) Берём исходный полный список приютов
            var filtered = _sheltersList.AsEnumerable();

            // 2) Фильтрация по открытой территории
            if (ShowOpenYardOnly)
            {
                filtered = filtered.Where(s => s.HasOpenYard);
            }

            // 3) Фильтрация по типу животного
            if (SelectedPetType != null)
            {
                Console.WriteLine("Проверка фильтра по типу:");
                foreach (var shelter in _sheltersList)
                {
                    var allPets = shelter.Filter(typeof(Pet)).ToList();
                    var typedPets = shelter.Filter(SelectedPetType).ToList();
                    
                    Console.WriteLine($"Приют {shelter.Name}: всего животных: {allPets.Count}, типа {SelectedPetType.Name}: {typedPets.Count}");
                    
                    // Выводим информацию о типах для отладки
                    foreach (var pet in allPets)
                    {
                        Console.WriteLine($"  Животное {pet.Name}: тип из свойства = {pet.Type.Name}, тип объекта = {pet.GetType().Name}");
                        Console.WriteLine($"  {pet.Type.Name} == {SelectedPetType.Name}: {pet.Type == SelectedPetType}");
                    }
                }
                
                filtered = filtered.Where(s => s.Filter(SelectedPetType).Any());
            }

            // 4) Обновляем коллекцию
            var filteredList = filtered.ToList();
            Console.WriteLine($"После всех фильтров осталось приютов: {filteredList.Count}");
            
            Shelters = new ObservableCollection<Shelter>(filteredList);

            // 5) Устанавливаем выбранный приют
            if (Shelters.Any())
                SelectedShelter = Shelters.First();
            else
                SelectedShelter = null;
        }

        private bool CanShowPets() => true;

        private void ShowPets()
        {
            if (SelectedShelter == null)
            {
                MessageBox.Show("Не выбран приют.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pets = SelectedShelter.Filter(SelectedPetType ?? typeof(Pet));

            if (ShowOpenYardOnly)
                pets = pets.Where(p => !p.Claustrophobic || SelectedShelter.HasOpenYard);

            if (pets.Any())
            {
                string fileName = SaveReport(pets);

                if (!string.IsNullOrEmpty(fileName))
                {
                    string extension = SelectedReportFormat == ReportFormat.Json ? "json" : "xml";
                    MessageBox.Show($"Отчет успешно сохранен как: {fileName}.{extension}", 
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
    
                var petsVm = new PetsViewModel(SelectedShelter, SelectedPetType, ShowOpenYardOnly);
                var petsView = new PetsView { DataContext = petsVm };
                petsView.Show();
            }
            else
            {
                MessageBox.Show("Нет питомцев по заданным критериям.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowStatistics()
        {
            if (SelectedShelter == null) return;
            var statsVm = new StatisticsViewModel(SelectedShelter);
            var statsView = new StatisticsView { DataContext = statsVm };
            statsView.Show();
        }

        // Метод для сохранения отчета
        public string SaveReport(IEnumerable<Pet> pets)
        {
            try
            {
                _reportCounter++;

                string dateString = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                string fileName = $"Подборка_№{_reportCounter}_от_{dateString}";

                if (SelectedReportFormat == ReportFormat.Json)
                {
                    _jsonSerializer.Serialize(fileName, pets.ToList());
                }
                else
                {
                    _xmlSerializer.Serialize(fileName, pets.ToList());
                }

                return fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Метод для конвертации отчетов из одного формата в другой
        private void ConvertReports(ReportFormat fromFormat, ReportFormat toFormat)
        {
            if (fromFormat == toFormat)
            {
                MessageBox.Show("Исходный и целевой форматы совпадают. Конвертация не требуется.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string fromExtension = fromFormat == ReportFormat.Json ? "json" : "xml";
            string toExtension = toFormat == ReportFormat.Json ? "json" : "xml";
            string searchPattern = $"Подборка_*.{fromExtension}";
            
            try
            {
                // Используем тот же базовый путь, что и сериализаторы
                string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                
                // Проверяем существование директории
                if (!Directory.Exists(baseDirectory))
                {
                    MessageBox.Show($"Директория с отчетами не найдена: {baseDirectory}",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                // Ищем файлы в правильной директории
                string[] files = Directory.GetFiles(baseDirectory, searchPattern);
                
                if (files.Length == 0)
                {
                    MessageBox.Show($"Не найдены отчеты в формате {fromExtension} для конвертации.",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                int convertedCount = 0;
                foreach (string file in files)
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                    
                    if (fromFormat == ReportFormat.Json && toFormat == ReportFormat.Xml)
                    {
                        var data = _jsonSerializer.Deserialize<List<Pet>>(fileNameWithoutExt);
                        _xmlSerializer.Serialize(fileNameWithoutExt, data);
                        convertedCount++;
                    }
                    else if (fromFormat == ReportFormat.Xml && toFormat == ReportFormat.Json)
                    {
                        var data = _xmlSerializer.Deserialize<List<Pet>>(fileNameWithoutExt);
                        _jsonSerializer.Serialize(fileNameWithoutExt, data);
                        convertedCount++;
                    }
                }
                
                MessageBox.Show($"Успешно конвертировано {convertedCount} отчетов из формата {fromFormat} в {toFormat}.",
                    "Конвертация завершена", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при конвертации отчетов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}