using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Helper;
using Lab_10_PetShelter.Views;
using PetShelter.Model.Core;
using PetShelter.Model.Data;

namespace Lab_10_PetShelter.ViewModels
{
    public class PetsViewModel : BaseViewModel
    {
        private readonly Shelter _shelter;
        private ObservableCollection<Pet> _filteredPets;
        private Pet _selectedPet;
        private readonly Type _selectedPetType;
        private readonly bool _showOpenYardOnly;
        
        private readonly SerializerBase _jsonSerializer;
        private readonly SerializerBase _xmlSerializer;
        
        private ReportFormat _selectedReportFormat = ReportFormat.Json;
        public ReportFormat SelectedReportFormat
        {
            get => _selectedReportFormat;
            set => SetField(ref _selectedReportFormat, value);
        }
        
        public ObservableCollection<ReportFormat> ReportFormats { get; } = 
            new ObservableCollection<ReportFormat>
            {
                ReportFormat.Json,
                ReportFormat.Xml
            };
            
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
        public ICommand SaveReportCommand { get; }
        
        public PetsViewModel(Shelter shelter, Type petType, bool showOpenYardOnly)
        {
            _shelter = shelter;
            _showOpenYardOnly = showOpenYardOnly;
            _selectedPetType = petType;
            
            // Инициализируем сериализаторы
            _jsonSerializer = new JsonSerializer();
            _xmlSerializer = new XmlSerializer();
            
            AddPetCommand = new RelayCommand(AddPet, CanAddPet);
            RemovePetCommand = new RelayCommand(RemovePet, CanRemovePet);
            SaveReportCommand = new RelayCommand(SaveReport, () => FilteredPets != null && FilteredPets.Any());

            FilterPets(petType, showOpenYardOnly);
        }
        
        private void FilterPets(Type petType, bool showOpenYardOnly)
        {
            // Получаем всех питомцев
            var pets = petType == null ? 
                _shelter.Filter(typeof(Pet)) : 
                _shelter.Filter(petType);

            if (showOpenYardOnly)
                pets = pets.Where(p => !p.Claustrophobic || _shelter.HasOpenYard);

            FilteredPets = new ObservableCollection<Pet>(pets);
        }
        
        private bool CanAddPet() => _shelter != null;
        
        private bool CanRemovePet() => SelectedPet != null;
        
        private void AddPet()
        {
            try
            {
                // Создаем окно перед тем, как создать ViewModel
                var addPetView = new AddPetView();
                
                // Создаем ViewModel, передавая ему окно и приют
                var vm = new AddPetViewModel(addPetView, _shelter);
                
                // Устанавливаем DataContext окна
                addPetView.DataContext = vm;
                
                // Показываем окно
                bool? result = addPetView.ShowDialog();
                
                // Если диалог был закрыт с результатом true, обновляем список питомцев
                if (result == true)
                {
                    FilterPets(_selectedPetType, _showOpenYardOnly);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении питомца: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void RemovePet()
        {
            if (SelectedPet == null)
                return;

            try
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить питомца {SelectedPet.Name}?",
                        "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _shelter.Remove(SelectedPet);
                    FilteredPets.Remove(SelectedPet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении питомца: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void SaveReport()
        {
            try
            {
                string dateString = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                string fileName = $"Отчет_питомцы_{_shelter.Name}_{dateString}";
                
                if (SelectedReportFormat == ReportFormat.Json)
                {
                    _jsonSerializer.Serialize(fileName, FilteredPets.ToList());
                }
                else
                {
                    _xmlSerializer.Serialize(fileName, FilteredPets.ToList());
                }
                
                string extension = SelectedReportFormat == ReportFormat.Json ? "json" : "xml";
                MessageBox.Show($"Отчет успешно сохранен как: {fileName}.{extension}", 
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}