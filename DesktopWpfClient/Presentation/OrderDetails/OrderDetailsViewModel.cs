using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DesktopWpfClient.Data.Repositories;
using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Presentation.Navigation;
using DesktopWpfClient.Presentation.IncomeSelector;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace DesktopWpfClient.Presentation.OrderDetails;

/// <summary>
/// ViewModel для экрана деталей заказа. 
/// Отвечает за отображение платежей, выбор дохода и создание новых платежей.
/// </summary>
public partial class OrderDetailsViewModel(
    NavigationService navigation,
    PaymentsRepository paymentsRepository,
    OrdersRepository ordersRepository,
    IncomesRepository incomesRepository,
    Order order
) : ObservableValidator, INavigationTarget<Order>, INavigationResultListener<Income> {

    /// <summary>
    /// Заказ, для которого отображаются детали и платежи.
    /// </summary>
    [ObservableProperty]
    private Order order = order;

    /// <summary>
    /// Выбранный доход для выполнения платежа.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreatePaymentCommand))]
    private Income? income = null;

    /// <summary>
    /// Коллекция платежей, связанных с текущим заказом.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreatePaymentCommand))]
    private ObservableCollection<Payment> payments = [];

    /// <summary>
    /// Текстовая сумма платежа, которую вводит пользователь.
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(CreatePaymentCommand))]
    [CustomValidation(typeof(OrderDetailsViewModel), nameof(ValidatePaymentAmount))]
    private string paymentAmountText = "0";

    /// <summary>
    /// Валидация введенной суммы платежа.
    /// Проверяет корректность данных и соответствие условиям.
    /// </summary>
    /// <param name="text">Текстовое представление суммы.</param>
    /// <param name="context">Контекст валидации.</param>
    /// <returns>Результат валидации.</returns>
    public static ValidationResult? ValidatePaymentAmount(string text, ValidationContext context) {
        var instance = (OrderDetailsViewModel)context.ObjectInstance;
        if (!decimal.TryParse(text, out var amount)) {
            return new ValidationResult("Не число.");
        }
        if (amount <= 0) {
            return new("Нельзя переводить отрицательную сумму");
        }
        if (amount > (instance.Income?.RemainingAmount ?? 0)) {
            return new("Нельзя перевести больше средств, чем осталось на счету.");
        }
        if (amount > instance.Order.TotalAmount - instance.Order.PaidAmount) {
            return new("Нельзя перевести больше средств, чем осталось оплатить.");
        }
        return ValidationResult.Success;
    }

    /// <summary>
    /// Обработка события навигации. Загружает платежи для текущего заказа.
    /// </summary>
    /// <param name="_">Игнорируемый параметр заказа.</param>
    public async void OnNavigatedTo(Order _) {
        var result = await paymentsRepository.GetOrderPaymentsAsync(Order.OrderID);
        if (result.Status == Status.Success) {
            Payments = new(result.Value);
        } else if (result.Status == Status.ApiError) {
            MessageBox.Show("Ошибка от сервера");
        } else {
            MessageBox.Show("Нет связи с сервером");
        }
    }

    /// <summary>
    /// Обновляет данные заказа, платежей и выбранного дохода.
    /// </summary>
    private async void RefreshData() {
        var newPayments = await paymentsRepository.GetOrderPaymentsAsync(Order.OrderID);
        var newOrder = await ordersRepository.GetOrderAsync(Order.OrderID);
        Result<Income?>? newIncome = null;
        if (Income != null) {
            newIncome = await incomesRepository.GetIncomeAsync(Income.IncomeID);
        }

        if (newPayments.Status == Status.Success) {
            Payments = new(newPayments.Value);
        }
        if (newOrder.Status == Status.Success && newOrder.Value != null) {
            Order = newOrder.Value;
        }
        if (newIncome != null && newIncome.Status == Status.Success) {
            Income = newIncome.Value;
        }
        if (newPayments.Status != Status.Success || newOrder.Status != Status.Success || (newIncome != null && newIncome.Status != Status.Success)) {
            MessageBox.Show("Не удалось обновить данные");
        }
    }

    /// <summary>
    /// Обработка результата навигации. Устанавливает выбранный доход.
    /// </summary>
    /// <param name="result">Выбранный доход.</param>
    public void OnNavigationResult(Income result) {
        Income = result;
        ValidateProperty(PaymentAmountText, nameof(PaymentAmountText));
    }

    /// <summary>
    /// Навигация на экран выбора дохода.
    /// </summary>
    [RelayCommand]
    private void NavigateToIncomeSelector() {
        navigation.NavigateTo<IncomeSelectorViewModel, IncomeFilter?>(IncomeFilter.Filled);
    }

    /// <summary>
    /// Проверяет возможность создания платежа.
    /// </summary>
    private bool CanCreatePayment => Income != null && !HasErrors;

    /// <summary>
    /// Создает новый платеж и обновляет данные.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCreatePayment))]
    private async Task CreatePayment() {
        if (Income == null || !decimal.TryParse(PaymentAmountText, out var amount)) {
            return;
        }
        var status = await paymentsRepository.AddPaymentAsync(new(
            OrderID: Order.OrderID,
            IncomeID: Income.IncomeID,
            PaymentAmount: amount,
            ExpectedRemainingAmount: Income.RemainingAmount,
            ExpectedPaidAmount: Order.PaidAmount
        ));
        if (status == Status.Success) {
            RefreshData();
        } else if (status == Status.ApiError) {
            RefreshData();
            MessageBox.Show("Ошибка от сервера");
        } else {
            MessageBox.Show("Нет связи с сервером");
        }
    }
}
