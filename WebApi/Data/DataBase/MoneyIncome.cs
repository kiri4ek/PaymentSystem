using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Data.DataBase;

[Table("MoneyIncome")]
public class MoneyIncome {
    /// <summary>
    /// Уникальный идентификатор прихода.
    /// </summary>
    [Key]
    public int IncomeID { get; set; }

    /// <summary>
    /// Дата прихода суммы денег.
    /// </summary>
    public DateTime IncomeDate { get; set; }

    /// <summary>
    /// Сумма прихода.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Остаток от прихода.
    /// </summary>
    public decimal RemainingAmount { get; set; }

    /// <summary>
    /// Коллекция платежей заказов.
    /// </summary>
    [JsonIgnore]
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
