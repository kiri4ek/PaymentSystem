using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.DataBase;

[Table("Orders")]
public class Order {
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    [Key]
    public int OrderID { get; set; }

    /// <summary>
    /// Дата создания заказа.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Общая стоимость заказа.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Оплаченная сумма заказа.
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Коллекция платежей заказов.
    /// </summary>
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
