using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopWpfClient.Data.Models;
using DesktopWpfClient.Presentation.Navigation;

namespace DesktopWpfClient.Presentation.IncomeDetails;

public partial class IncomeDetailsViewModel(
    NavigationService navigation,
    Income income
) : ObservableObject, INavigationTarget<Income> {

    [ObservableProperty]
    private Income income = income;

    public void OnNavigatedTo(Income? args) {
        if (args != null) {
            Income = args;
        }
    }

    [RelayCommand]
    private void EditIncome() {
        if (Income != null) {

        }
    }
}
