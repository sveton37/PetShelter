namespace PetShelter.Model.Data;

/// <summary>
/// Перечисление, определяющее поддерживаемые форматы отчетов.
/// Используется для выбора формата сохранения и конвертации отчетов.
/// </summary>
public enum ReportFormat
{
    /// <summary>
    /// Формат JSON (JavaScript Object Notation).
    /// </summary>
    /// <remarks>
    /// Использует сериализатор Newtonsoft.Json для обработки данных.
    /// </remarks>
    Json,
    
    /// <summary>
    /// Формат XML (eXtensible Markup Language).
    /// </summary>
    /// <remarks>
    /// Использует стандартный .NET XmlSerializer для обработки данных.
    /// </remarks>
    Xml
}