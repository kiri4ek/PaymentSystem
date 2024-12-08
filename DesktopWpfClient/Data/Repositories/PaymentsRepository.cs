using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Data.Requests;
using DesktopWpfClient.Utilities;

namespace DesktopWpfClient.Data.Repositories;

/// <summary>
/// Репозиторий для работы с платежами.
/// Обеспечивает взаимодействие с API для выполнения операций получения и добавления платежей.
/// </summary>
public class PaymentsRepository(IApiService api) {
    /// <summary>
    /// Получает список платежей для конкретного заказа.
    /// </summary>
    /// <param name="orderId">Идентификатор заказа, для которого нужно получить платежи.</param>
    /// <returns>
    /// Результат выполнения запроса, содержащий список платежей или статус ошибки.
    /// </returns>
    public async Task<Result<List<Payment>>> GetOrderPaymentsAsync(int orderId) {
        return await RequestHelper.DoRequest(async() => await api.GetOrderPaymentsAsync(orderId), []);
    }

    /// <summary>
    /// Добавляет новый платеж для заказа.
    /// </summary>
    /// <param name="request">Объект запроса, содержащий данные о новом платеже.</param>
    /// <returns>
    /// Статус выполнения запроса, показывающий успех или наличие ошибки.
    /// </returns>
    public async Task<Status> AddPaymentAsync(CreatePaymentRequest request) {
        return await RequestHelper.DoRequest(async() => await api.AddPaymentAsync(request));
    }
}
