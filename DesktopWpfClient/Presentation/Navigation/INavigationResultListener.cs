namespace DesktopWpfClient.Presentation.Navigation;

/// <summary> Получает уведомление о возврате на этот экран без передачи аргументов. </summary>
public interface INavigationResultListener {
    void OnNavigationResult();
}

/// <summary> Получает уведомление о возврате на этот экран с передачей аргументов. </summary>
/// <typeparam name="TArgs">Аргументы передаваемые при возврате.</typeparam>
public interface INavigationResultListener<TArgs> {
    void OnNavigationResult(TArgs args);
}
