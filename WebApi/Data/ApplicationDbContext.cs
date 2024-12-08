using Microsoft.EntityFrameworkCore;
using WebApi.Data.DataBase;

namespace WebApi.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options) {
    /// <summary>
    /// Коллекция заказов, хранящая информацию о заказах пользователей.
    /// </summary>
    public DbSet<Order> Orders { get; set; } = null!;

    /// <summary>
    /// Коллекция приходов, хранящая информацию о пользовательских транзакциях.
    /// </summary>
    public DbSet<MoneyIncome> MoneyIncome { get; set; } = null!;

    /// <summary>
    /// Коллекция платежей, хранящая информацию о назначение каждого прихода к заказу.
    /// </summary>
    public DbSet<Payment> Payments { get; set; } = null!;

    /// <summary>
    /// Конфигурирует конвенции для модели, добавляя конвенцию для триггеров.
    /// </summary>
    /// <param name="configurationBuilder">Объект для настройки конфигурации модели.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }
}
