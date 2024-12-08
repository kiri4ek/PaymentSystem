namespace DesktopWpfClient.Data.Models;

/// <summary> Статус запроса к серверу. </summary>
public enum Status {
    /// <summary> Сервер принял запрос и вернул результат. </summary>
    Success,
    /// <summary> Сервер принял запрос, но вернул ошибку. </summary>
    ApiError,
    /// <summary> Сервер отказал в подключении. </summary>
    InternetError,
    /// <summary> Не удалось связаться с сервером. </summary>
    ConnectionError,
}
