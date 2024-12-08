using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Utilities;

namespace DesktopWpfClient.Data.Repositories;

/// <summary>
/// Репозиторий для работы с данными о заказах.
/// Обеспечивает взаимодействие с API для выполнения операций получения и добавления данных.
/// </summary>
public class OrdersRepository(IApiService api) {
    /// <summary>
    /// Получает список всех заказов.
    /// </summary>
    /// <returns>
    /// Результат выполнения запроса, содержащий список заказов или статус ошибки.
    /// </returns>
    public async Task<Result<List<Order>>> GetOrdersAsync() {
        return await RequestHelper.DoRequest(async() => await api.GetOrdersAsync(), []);
    }

    /// <summary>
    /// Получает данные о конкретном заказе по его идентификатору.
    /// </summary>
    /// <param name="orderID">Идентификатор заказа.</param>
    /// <returns>
    /// Результат выполнения запроса, содержащий объект <see cref="Order"/> или статус ошибки.
    /// </returns>
    public async Task<Result<Order?>> GetOrderAsync(int orderID) {
        return await RequestHelper.DoRequest(async () => await api.GetOrdersAsync(orderID), null);
    }

    /// <summary>
    /// Добавляет новый заказ.
    /// </summary>
    /// <param name="order">Данные о заказе, который нужно добавить.</param>
    /// <returns>
    /// Статус выполнения запроса, показывающий успех или наличие ошибки.
    /// </returns>
    public async Task<Status> AddOrderAsync(Order order) {
        return await RequestHelper.DoRequest(async () => await api.AddOrderAsync(order));
    }
}
