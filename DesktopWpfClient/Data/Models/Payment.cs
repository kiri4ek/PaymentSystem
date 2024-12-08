namespace DesktopWpfClient.Data.Models;

/// <summary>
/// Модель данных для представления платежа.
/// </summary>
public record Payment(
    /// <summary>
    /// Уникальный идентификатор платежа.
    /// </summary>
    int PaymentID,

    /// <summary>
    /// Идентификатор заказа, к которому относится платеж.
    /// </summary>
    int OrderID,

    /// <summary>
    /// Идентификатор прихода денег, к которому привязан платеж.
    /// </summary>
    int IncomeID,

    /// <summary>
    /// Сумма платежа.
    /// </summary>
    decimal PaymentAmount
);
