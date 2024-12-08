namespace WebApi.Data.Requests;

public record CreateIncomeRequest(
    /// <summary>
    /// Сумма прихода.
    /// </summary>
    decimal TotalAmount,

    /// <summary>
    /// Остаток от прихода.
    /// </summary>
    decimal RemainingAmount
);