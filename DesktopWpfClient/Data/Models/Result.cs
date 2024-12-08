namespace DesktopWpfClient.Data.Models;

/// <summary> Обёртка результата запроса к серверу, хранящая полученные данные и/или статус. </summary>
public record Result<T>(
    T Value,
    Status Status
);
