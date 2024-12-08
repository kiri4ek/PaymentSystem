using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Data.Repositories;
using DesktopWpfClient.Presentation.Navigation;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopWpfClient.Presentation.IncomeSelector;


/// <summary>
/// ViewModel для выбора прихода денег.
/// Отображает список доступных приходов и позволяет пользователю выбрать один из них.
/// </summary>
public partial class IncomeSelectorViewModel(
    NavigationService navigation,
    IncomesRepository repository,
    IncomeFilter? filter
) : ObservableObject, INavigationTarget<IncomeFilter?> {

    /// <summary>
    /// Коллекция доступных приходов денег.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Income> incomes = [];

    /// <summary>
    /// Выбранный приход денег.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NavigateBackWithResultCommand))]
    private Income? selectedIncome = null;

    /// <summary>
    /// Указывает, можно ли вернуться с выбранным приходом денег.
    /// </summary>
    private bool CanReturnIncome => SelectedIncome != null;

    /// <summary>
    /// Метод вызывается при навигации к этому экрану.
    /// Загружает список доступных приходов с учетом переданного фильтра.
    /// </summary>
    /// <param name="filter">Фильтр для загрузки данных о приходах.</param>
    public void OnNavigatedTo(IncomeFilter? filter) {
        LoadIncomes();
    }

    /// <summary>
    /// Загружает список приходов денег из репозитория.
    /// </summary>
    private async void LoadIncomes() {
        var result = await repository.GetIncomesAsync(filter);
        if (result.Status == Status.Success) {
            Incomes = new(result.Value);
        } else if (result.Status == Status.ApiError) {
            MessageBox.Show("Ошибка от сервера");
        } else {
            MessageBox.Show("Нет связи с сервером");
        }
    }

    /// <summary>
    /// Возвращается на предыдущий экран с выбранным приходом денег.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanReturnIncome))]
    private void NavigateBackWithResult() {
        navigation.Back(SelectedIncome!);
    }
}
