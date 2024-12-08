using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Utilities;

namespace DesktopWpfClient.Data.Repositories;

/// <summary>
/// Репозиторий для работы с данными о приходах денег.
/// Обеспечивает взаимодействие с API для выполнения операций получения и добавления данных.
/// </summary>
public class IncomesRepository(IApiService api) {
    /// <summary>
    /// Получает список приходов денег с возможной фильтрацией.
    /// </summary>
    /// <param name="filter">Фильтр для выборки данных о приходах.</param>
    /// <returns>
    /// Результат выполнения запроса, содержащий список приходов денег или статус ошибки.
    /// </returns>
    public async Task<Result<List<Income>>> GetIncomesAsync(IncomeFilter? filter) {
        return await RequestHelper.DoRequest(async () => await api.GetIncomesAsync(filter), []);
    }

    /// <summary>
    /// Получает данные о конкретном приходе денег по его идентификатору.
    /// </summary>
    /// <param name="incomeID">Идентификатор прихода.</param>
    /// <returns>
    /// Результат выполнения запроса, содержащий объект <see cref="Income"/> или статус ошибки.
    /// </returns>
    public async Task<Result<Income?>> GetIncomeAsync(int incomeID) {
        return await RequestHelper.DoRequest(async () => await api.GetIncomeAsync(incomeID), null);
    }

    /// <summary>
    /// Добавляет новый приход денег.
    /// </summary>
    /// <param name="income">Данные о приходе, который нужно добавить.</param>
    /// <returns>
    /// Статус выполнения запроса, показывающий успех или наличие ошибки.
    /// </returns>
    public async Task<Status> AddIncomeAsync(Income income) {
        return await RequestHelper.DoRequest(async () => await api.AddIncomeAsync(income));
    }
}
