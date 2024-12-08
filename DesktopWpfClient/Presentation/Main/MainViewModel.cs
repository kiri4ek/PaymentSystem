using CommunityToolkit.Mvvm.Input;
using DesktopWpfClient.Presentation.Navigation;
using DesktopWpfClient.Presentation.OrdersList;
using DesktopWpfClient.Presentation.IncomesList;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopWpfClient.Presentation.Main;

/// <summary>
/// ViewModel для главного окна приложения.
/// Управляет навигацией между экранами.
/// </summary>
public partial class MainViewModel : ObservableObject {
    /// <summary>
    /// Сервис навигации, управляющий переключением экранов.
    /// </summary>
    public NavigationService Navigation { get; }

    /// <summary>
    /// Конструктор ViewModel. Устанавливает экран по умолчанию и инициализирует навигацию.
    /// </summary>
    /// <param name="navigationService">Сервис навигации.</param>
    public MainViewModel(NavigationService navigationService) {
        navigationService.NewRootScreen<OrdersListViewModel>();
        Navigation = navigationService;
    }

    /// <summary>
    /// Переключение на экран со списком заказов.
    /// </summary>
    [RelayCommand]
    private void NavigateToOrders() {
        Navigation.NewRootScreen<OrdersListViewModel>();
    }

    /// <summary>
    /// Переключение на экран со списком приходов.
    /// </summary>
    [RelayCommand]
    private void NavigateToIncomes() {
        Navigation.NewRootScreen<IncomesListViewModel>();
    }
}
