using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DesktopWpfClient.Data.Repositories;
using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Presentation.Navigation;
using DesktopWpfClient.Presentation.OrderDetails;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace DesktopWpfClient.Presentation.OrdersList;

/// <summary>
/// ViewModel для экрана со списком заказов. 
/// Отвечает за отображение списка, добавление новых заказов и открытие деталей заказа.
/// </summary>
public partial class OrdersListViewModel(
    NavigationService navigation,
    OrdersRepository repository
) : ObservableObject, INavigationTarget, INavigationResultListener {

    /// <summary>
    /// Коллекция заказов, отображаемых в списке.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Order> orders = [];

    /// <summary>
    /// Выбранный заказ из списка.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenDetailsCommand))]
    private Order? selectedOrder = null;

    /// <summary>
    /// Указывает, можно ли открыть детали выбранного заказа.
    /// </summary>
    private bool CanOpenDetails => SelectedOrder != null;

    /// <summary>
    /// Метод, вызываемый при навигации на этот экран. Загружает список заказов.
    /// </summary>
    public void OnNavigatedTo() {
        LoadOrders();
    }

    /// <summary>
    /// Метод, вызываемый после завершения навигации назад. Перезагружает список заказов.
    /// </summary>
    public void OnNavigationResult() {
        LoadOrders();
    }

    /// <summary>
    /// Загружает список заказов из репозитория.
    /// </summary>
    private async void LoadOrders() {
        var result = await repository.GetOrdersAsync();
        if (result.Status == Status.Success) {
            Orders = new(result.Value);
        } else if (result.Status == Status.ApiError) {
            MessageBox.Show("Ошибка от сервера");
        } else {
            MessageBox.Show("Нет связи с сервером");
        }
    }

    /// <summary>
    /// Открывает экран с деталями выбранного заказа.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanOpenDetails))]
    private void OpenDetails() {
        navigation.NavigateTo<OrderDetailsViewModel, Order>(SelectedOrder!);
    }

    /// <summary>
    /// Добавляет новый заказ и обновляет список заказов.
    /// </summary>
    [RelayCommand]
    private async Task AddOrder() {
        var newOrder = new Order(
            OrderID: 0,
            OrderDate: DateTime.Now,
            TotalAmount: 1000m, // Пример данных
            PaidAmount: 0m
        );

        var status = await repository.AddOrderAsync(newOrder);
        if (status == Status.Success) {
            Orders.Add(newOrder);
            LoadOrders();
        } else if (status == Status.ApiError) {
            MessageBox.Show("Ошибка от сервера");
        } else {
            MessageBox.Show("Нет связи с сервером");
        }
    }
}
