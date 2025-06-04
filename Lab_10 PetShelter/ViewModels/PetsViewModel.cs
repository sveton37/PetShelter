using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Lab_10_PetShelter.Helper;
using Lab_10_PetShelter.Views;
using PetShelter.Model.Core;
using PetShelter.Model.Data;

namespace Lab_10_PetShelter.ViewModels;

/// <summary>
/// Модель представления для окна со списком животных в приюте.
/// Обеспечивает функциональность просмотра, добавления и удаления животных.
/// </summary>
public class PetsViewModel : BaseViewModel
{
    /// <summary>
    /// Приют, к которому относятся отображаемые животные.
    /// </summary>
    private readonly Shelter _shelter;
    
    /// <summary>
    /// Отфильтрованная коллекция животных для отображения.
    /// </summary>
    private ObservableCollection<Pet> _filteredPets;
    
    /// <summary>
    /// Выбранное животное в списке.
    /// </summary>
    private Pet _selectedPet;
    
    /// <summary>
    /// Тип животных для фильтрации (может быть null для отображения всех).
    /// </summary>
    private readonly Type _selectedPetType;
    
    /// <summary>
    /// Флаг фильтрации: показывать только животных, подходящих для приютов с открытой территорией.
    /// </summary>
    private readonly bool _showOpenYardOnly;

    /// <summary>
    /// Сериализатор для работы с JSON-файлами.
    /// </summary>
    private readonly SerializerBase _jsonSerializer;
    
    /// <summary>
    /// Сериализатор для работы с XML-файлами.
    /// </summary>
    private readonly SerializerBase _xmlSerializer;

    /// <summary>
    /// Выбранный формат отчета (JSON или XML).
    /// </summary>
    private ReportFormat _selectedReportFormat = ReportFormat.Json;

    /// <summary>
    /// Выбранный формат отчета с уведомлением об изменении.
    /// </summary>
    public ReportFormat SelectedReportFormat
    {
        get => _selectedReportFormat;
        set => SetField(ref _selectedReportFormat, value);
    }

    /// <summary>
    /// Коллекция доступных форматов отчетов для выбора.
    /// </summary>
    public ObservableCollection<ReportFormat> ReportFormats { get; } =
        new ObservableCollection<ReportFormat>
        {
            ReportFormat.Json,
            ReportFormat.Xml
        };

    /// <summary>
    /// Отфильтрованная коллекция животных для отображения в UI.
    /// </summary>
    public ObservableCollection<Pet> FilteredPets
    {
        get => _filteredPets;
        set => SetField(ref _filteredPets, value);
    }
    
    /// <summary>
    /// Выбранное животное в списке.
    /// </summary>
    public Pet SelectedPet
    {
        get => _selectedPet;
        set => SetField(ref _selectedPet, value);
    }
    
    /// <summary>
    /// Команда для добавления нового животного в приют.
    /// </summary>
    public ICommand AddPetCommand { get; }
    
    /// <summary>
    /// Команда для удаления выбранного животного из приюта.
    /// </summary>
    public ICommand RemovePetCommand { get; }
    
    /// <summary>
    /// Команда для сохранения отчета о животных.
    /// </summary>
    public ICommand SaveReportCommand { get; }

    /// <summary>
    /// Конструктор PetsViewModel.
    /// </summary>
    /// <param name="shelter">Приют, животных которого нужно отобразить</param>
    /// <param name="petType">Тип животных для фильтрации (может быть null для всех)</param>
    /// <param name="showOpenYardOnly">Флаг фильтрации по совместимости с открытой территорией</param>
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

    /// <summary>
    /// Фильтрует животных в приюте по заданным критериям.
    /// </summary>
    /// <param name="petType">Тип животных для фильтрации</param>
    /// <param name="showOpenYardOnly">Флаг фильтрации по совместимости с открытой территорией</param>
    private void FilterPets(Type petType, bool showOpenYardOnly)
    {
        // Получаем всех питомцев
        var pets = petType == null ? _shelter.Filter(typeof(Pet)) : _shelter.Filter(petType);

        if (showOpenYardOnly)
            pets = pets.Where(p => !p.Claustrophobic || _shelter.HasOpenYard);

        FilteredPets = new ObservableCollection<Pet>(pets);
    }

    /// <summary>
    /// Проверяет, можно ли добавить новое животное.
    /// </summary>
    /// <returns>true, если приют существует</returns>
    private bool CanAddPet() => _shelter != null;

    /// <summary>
    /// Проверяет, можно ли удалить выбранное животное.
    /// </summary>
    /// <returns>true, если выбрано животное в списке</returns>
    private bool CanRemovePet() => SelectedPet != null;

    /// <summary>
    /// Добавляет новое животное в приют через диалоговое окно.
    /// </summary>
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

    /// <summary>
    /// Удаляет выбранное животное из приюта.
    /// </summary>
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

    /// <summary>
    /// Сохраняет отчет о животных в файл.
    /// </summary>
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