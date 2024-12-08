namespace WebApi.Data.Requests;

public record CreateOrderRequest(
    /// <summary>
    /// Общая стоимость заказа.
    /// </summary>
    decimal TotalAmount,

    /// <summary>
    /// Оплаченная сумма заказа.
    /// </summary>
    decimal PaidAmount
);
