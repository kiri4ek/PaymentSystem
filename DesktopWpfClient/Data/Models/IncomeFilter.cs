namespace DesktopWpfClient.Data.Models;

/// <summary>
/// Перечисление для фильтрации приходов денег.
/// </summary>
public enum IncomeFilter {
    /// <summary>
    /// Фильтр, который возвращает пустые (незаполненные) приходы.
    /// </summary>
    Empty,

    /// <summary>
    /// Фильтр, который возвращает заполненные (с остатком) приходы.
    /// </summary>
    Filled,
}
