namespace DesktopWpfClient.Data.Models;

public record Order(
    int OrderID,
    DateTime OrderDate,
    decimal TotalAmount,
    decimal PaidAmount
);
