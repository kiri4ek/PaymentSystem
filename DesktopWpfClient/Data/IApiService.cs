using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Data.Requests;
using Refit;

namespace DesktopWpfClient.Data;

public interface IApiService {
    // Получение списка заказов
    [Get("/orders")]
    public Task<List<Order>> GetOrdersAsync();

    // Получение заказа по id
    [Get("/orders/{id}")]
    public Task<Order> GetOrdersAsync([AliasAs("id")] int orderId);

    // Добавление нового заказа
    [Post("/orders")]
    public Task AddOrderAsync([Body] Order order);

    // Получение списка приходов денег
    [Get("/incomes")]
    public Task<List<Income>> GetIncomesAsync([AliasAs("filter")] IncomeFilter? filter);

    // Получение прихода денег по id
    [Get("/incomes/{id}")]
    public Task<Income> GetIncomeAsync([AliasAs("id")] int incomeId);

    // Добавление нового прихода денег
    [Post("/incomes")]
    public Task AddIncomeAsync([Body] Income income);

    // Получение списка платежей по заказу
    [Get("/payments")]
    public Task<List<Payment>> GetOrderPaymentsAsync(int orderId);

    // Добавление платежа
    [Post("/payments")]
    public Task AddPaymentAsync([Body] CreatePaymentRequest payment);
}
