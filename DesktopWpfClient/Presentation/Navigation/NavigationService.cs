using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DesktopWpfClient.Presentation.Navigation;

/// <summary>
/// Сервис для управления навигацией между экранами в приложении.
/// Обеспечивает переходы вперед, назад и передачу параметров между экранами.
/// </summary>
public partial class NavigationService(
    Func<Type, INavigationTarget> viewModelFactory,
    Func<Type, object?, object> viewModelFactoryWithArgs
) : ObservableObject {

    /// <summary>
    /// Стек для хранения истории экранов для возможности возврата.
    /// </summary>
    private readonly Stack<object> backStack = new();

    /// <summary>
    /// Текущий активный экран (ViewModel).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanGoBack))]
    [NotifyCanExecuteChangedFor(nameof(BackCommand))]
    private object? currentScreen = null;

    /// <summary>
    /// Указывает, можно ли вернуться на предыдущий экран.
    /// </summary>
    public bool CanGoBack => backStack.Count > 0;

    /// <summary>
    /// Устанавливает новый корневой экран, очищая историю переходов.
    /// </summary>
    /// <typeparam name="T">Тип экрана (ViewModel), который реализует <see cref="INavigationTarget"/>.</typeparam>
    public void NewRootScreen<T>() where T : INavigationTarget {
        var vm = (T)viewModelFactory(typeof(T));
        vm.OnNavigatedTo();
        backStack.Clear();
        CurrentScreen = vm;
    }

    /// <summary>
    /// Выполняет переход к новому экрану.
    /// </summary>
    /// <typeparam name="T">Тип экрана (ViewModel), который реализует <see cref="INavigationTarget"/>.</typeparam>
    public void NavigateTo<T>() where T : INavigationTarget {
        var vm = (T)viewModelFactory(typeof(T));
        vm.OnNavigatedTo();
        if (CurrentScreen != null) {
            backStack.Push(CurrentScreen);
        }
        CurrentScreen = vm;
    }

    /// <summary>
    /// Выполняет переход к новому экрану с передачей параметров.
    /// </summary>
    /// <typeparam name="T">Тип экрана (ViewModel), который реализует <see cref="INavigationTarget{TArgs}"/>.</typeparam>
    /// <typeparam name="TArgs">Тип передаваемых параметров.</typeparam>
    /// <param name="args">Параметры для передачи на новый экран.</param>
    public void NavigateTo<T, TArgs>(TArgs args) where T : INavigationTarget<TArgs> {
        var vm = (T)viewModelFactoryWithArgs(typeof(T), args);
        vm.OnNavigatedTo(args);
        if (CurrentScreen != null) {
            backStack.Push(CurrentScreen);
        }
        CurrentScreen = vm;
    }

    /// <summary>
    /// Возвращает пользователя на предыдущий экран, если это возможно.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanGoBack))]
    public void Back() {
        if (backStack.Count > 0) {
            var vm = backStack.Pop();
            if (vm is INavigationResultListener nav) {
                nav.OnNavigationResult();
            }
            CurrentScreen = vm;
        }
    }

    /// <summary>
    /// Возвращает пользователя на предыдущий экран с передачей результата.
    /// </summary>
    /// <typeparam name="TArgs">Тип передаваемых результатов.</typeparam>
    /// <param name="args">Результат для передачи предыдущему экрану.</param>
    public void Back<TArgs>(TArgs args) {
        if (backStack.Count > 0) {
            var vm = backStack.Pop();
            if (vm is INavigationResultListener<TArgs> nav) {
                nav.OnNavigationResult(args);
            }
            CurrentScreen = vm;
        }
    }
}
