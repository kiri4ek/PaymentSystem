namespace DesktopWpfClient.Data.Models;

/// <summary>
/// Модель данных для представления прихода денег.
/// </summary>
public record Income(
    /// <summary>
    /// Уникальный идентификатор прихода.
    /// </summary>
    int IncomeID,

    /// <summary>
    /// Дата прихода денег.
    /// </summary>
    DateTime IncomeDate,

    /// <summary>
    /// Общая сумма прихода денег.
    /// </summary>
    decimal TotalAmount,

    /// <summary>
    /// Остаточная сумма прихода денег, которая ещё не использована.
    /// </summary>
    decimal RemainingAmount
);
