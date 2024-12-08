namespace DesktopWpfClient.Data.Requests;

public record CreatePaymentRequest(
    /// <summary> Уникальный идентификатор заказа. </summary>
    int OrderID,

    /// <summary> Уникальный идентификатор прихода. </summary>
    int IncomeID,

    /// <summary> Сумма платежа. </summary>
    decimal PaymentAmount,

    /// <summary> Ожидаемый клиентом текущий остаток средств в приходе. </summary>
    decimal ExpectedRemainingAmount,

    /// <summary> Ожидаемый клиентом текущая оплаченная сумма заказа. </summary>
    decimal ExpectedPaidAmount
);
