using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_10_PetShelter.ViewModels;

/// <summary>
/// Базовый класс для всех моделей представления.
/// Реализует интерфейс INotifyPropertyChanged для уведомления об изменении свойств.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Событие, вызываемое при изменении свойства.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Вызывает событие PropertyChanged для указанного свойства.
    /// </summary>
    /// <param name="propertyName">Имя изменившегося свойства (устанавливается автоматически).</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Устанавливает новое значение для поля и вызывает событие PropertyChanged, 
    /// если значение изменилось.
    /// </summary>
    /// <typeparam name="T">Тип свойства</typeparam>
    /// <param name="field">Ссылка на поле, хранящее значение свойства</param>
    /// <param name="value">Новое значение</param>
    /// <param name="propertyName">Имя свойства (устанавливается автоматически)</param>
    /// <returns>true, если значение изменилось; false в противном случае</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}