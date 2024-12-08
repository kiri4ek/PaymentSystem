namespace DesktopWpfClient.Presentation.Navigation;

/// <summary>
/// Интерфейс необходимый только для более типо-безопасной навигации.<br/>
/// Для возможности открытия экрана класс должен унаследовать этот интерфейс или <see cref="INavigationTarget{TArgs}"/>.
/// На случай наследования обоих интерфейсов оповещает об используемом методе открытия.
/// </summary>
public interface INavigationTarget {
    public void OnNavigatedTo() { }
}

/// <summary>
/// Интерфейс неоходимый только для более типо-безопасной навигации, требует при открытии предоставить указанные аргументы.<br/>
/// Для возможности открытия экрана класс должен унаследовать этот интерфейс или <see cref="INavigationTarget"/>.
/// Предпочтительный способ получения аргументов - конструктор, но на случай наследования обоих интерфейсов оповещает об используемом методе открытия.
/// </summary>
/// <typeparam name="TArgs">Аргументы передаваемые при переходе.</typeparam>
public interface INavigationTarget<TArgs> {
    public void OnNavigatedTo(TArgs args) { }
}
