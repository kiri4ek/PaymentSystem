using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Data.DataBase;

[Table("Payments")]
public class Payment {
    /// <summary>
    /// Уникальный идентификатор платежа.
    /// </summary>
    [Key]
    public int PaymentID { get; set; }

    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public int OrderID { get; set; }

    /// <summary>
    /// Уникальный идентификатор прихода.
    /// </summary>
    public int IncomeID { get; set; }

    /// <summary>
    /// Сумма платежа.
    /// </summary>
    public decimal PaymentAmount { get; set; }

    [JsonIgnore]
    public Order Order { get; set; }
    public MoneyIncome Income { get; set; }
}
