using System.Windows.Input;

namespace Lab_10_PetShelter.Helper;

/// <summary>
/// Реализация интерфейса ICommand для связывания команд с методами в моделях представления.
/// Используется в паттерне MVVM для выполнения действий при взаимодействии с пользовательским интерфейсом.
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    /// Делегат, представляющий метод, который будет выполняться при вызове команды.
    /// </summary>
    private readonly Action _execute;
    
    /// <summary>
    /// Делегат, представляющий метод, определяющий, может ли команда быть выполнена.
    /// </summary>
    private readonly Func<bool> _canExecute;

    /// <summary>
    /// Событие, вызываемое при изменении возможности выполнения команды.
    /// </summary>
    /// <remarks>
    /// Использует встроенный в WPF механизм CommandManager.RequerySuggested
    /// для автоматического обновления состояния команд.
    /// </remarks>
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Конструктор класса RelayCommand.
    /// </summary>
    /// <param name="execute">Метод, который будет выполняться при вызове команды.</param>
    /// <param name="canExecute">
    /// Метод, определяющий, может ли команда быть выполнена. 
    /// Если null, команда всегда может быть выполнена.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если параметр execute равен null.
    /// </exception>
    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Определяет, может ли команда быть выполнена в текущем состоянии.
    /// </summary>
    /// <param name="parameter">Параметр, переданный команде (не используется в этой реализации).</param>
    /// <returns>True, если команда может быть выполнена; иначе false.</returns>
    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute();
    }

    /// <summary>
    /// Выполняет логику команды.
    /// </summary>
    /// <param name="parameter">Параметр, переданный команде (не используется в этой реализации).</param>
    public void Execute(object parameter)
    {
        _execute();
    }
}