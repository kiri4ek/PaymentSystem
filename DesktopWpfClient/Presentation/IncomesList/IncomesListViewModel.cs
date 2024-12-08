using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Data.Repositories;
using DesktopWpfClient.Presentation.Navigation;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopWpfClient.Presentation.IncomesList;

/// <summary>
/// ViewModel для управления списком приходов денег.
/// Отвечает за загрузку данных, добавление новых приходов и работу с выбранным приходом.
/// </summary>
public partial class IncomesListViewModel(
    IncomesRepository repository
) : ObservableObject, INavigationTarget {

    /// <summary>
    /// Фильтр для загрузки данных о приходах.
    /// </summary>
    [ObservableProperty]
    private IncomeFilter? filter = null;

    /// <summary>
    /// Коллекция приходов денег, отображаемых в списке.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Income> incomes = [];

    /// <summary>
    /// Выбранный приход денег из списка.
    /// </summary>
    [ObservableProperty]
    private Income? selectedIncome = null;


    /// <summary>
    /// Метод, вызываемый при навигации на этот экран.
    /// Загружает данные с использованием текущего фильтра.
    /// </summary>
    public void OnNavigatedTo() {
        LoadMoneyIncomes(Filter);
    }


    /// <summary>
    /// Загружает список приходов денег из репозитория.
    /// </summary>
    /// <param name="filter">Фильтр для загрузки данных о приходах.</param>
    private async void LoadMoneyIncomes(IncomeFilter? filter) {
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
    /// Добавляет новый приход денег и обновляет список.
    /// </summary>
    [RelayCommand]
    private async Task AddIncome() {
        var newIncome = new Income(
            IncomeID: 0,
            IncomeDate: DateTime.Now,
            TotalAmount: 800m,
            RemainingAmount: 470m
        );

        var status = await repository.AddIncomeAsync(newIncome);
        if (status == Status.Success) {
            Incomes.Add(newIncome);
        } else if (status == Status.ApiError) {
            MessageBox.Show("Ошибка от сервера");
        } else {
            MessageBox.Show("Нет связи с сервером");
        }
    }
}
