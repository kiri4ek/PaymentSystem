using DesktopWpfClient.Data.Models;
using Refit;
using System.Diagnostics;
using System.Net.Http;

namespace DesktopWpfClient.Utilities;

/// <summary>
/// Вспомогательный класс для выполнения HTTP-запросов с обработкой ошибок.
/// </summary>
public static class RequestHelper {
    /// <summary>
    /// Выполняет HTTP-запрос, обрабатывает ошибки и возвращает результат.
    /// </summary>
    /// <typeparam name="T">Тип ожидаемого результата.</typeparam>
    /// <param name="request">Функция, представляющая HTTP-запрос.</param>
    /// <param name="defaultValue">Значение по умолчанию, возвращаемое при ошибке.</param>
    /// <returns>
    /// Результат запроса, обёрнутый в объект <see cref="Result{T}"/>, содержащий статус выполнения.
    /// </returns>
    public static async Task<Result<T>> DoRequest<T>(Func<Task<T>> request, T defaultValue) {
        Status status = Status.Success;
        try {
            return new(await request(), status);
        } catch (ApiException e) {
            status = Status.ApiError;
            Trace.WriteLine($"Ошибка при получении заказов: {e.Message}");
        } catch (HttpRequestException e) {
            status = Status.ConnectionError;
            Trace.WriteLine($"Не удалось связаться с сервером: {e.Message}");
        } catch (TaskCanceledException e) {
            status = Status.InternetError;
            Trace.WriteLine($"Не удалось связаться с сервером: {e.Message}");
        }
        return new(defaultValue, status);
    }

    /// <summary>
    /// Выполняет HTTP-запрос без ожидания результата, обрабатывает ошибки и возвращает статус выполнения.
    /// </summary>
    /// <param name="request">Функция, представляющая HTTP-запрос.</param>
    /// <returns>Статус выполнения запроса.</returns>
    public static async Task<Status> DoRequest(Func<Task> request) {
        Status status = Status.Success;
        try {
            await request();
        } catch (ApiException e) {
            status = Status.ApiError;
            Trace.WriteLine($"Ошибка при получении заказов: {e.Message}");
        } catch (HttpRequestException e) {
            status = Status.ConnectionError;
            Trace.WriteLine($"Не удалось связаться с сервером: {e.Message}");
        } catch (TaskCanceledException e) {
            status = Status.InternetError;
            Trace.WriteLine($"Не удалось связаться с сервером: {e.Message}");
        }
        return status;
    }
}
