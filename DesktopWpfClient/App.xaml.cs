using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;
using Refit;
using DesktopWpfClient.Data;
using DesktopWpfClient.Data.Repositories;
using DesktopWpfClient.Presentation.Navigation;

namespace DesktopWpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public App() {
        ConfigureServices();
        InitializeComponent();
    }

    private static void ConfigureServices() {
        var services = new ServiceCollection()
            // Http
            .AddSingleton<HttpClient>(_ => new() {
                BaseAddress = new Uri("http://localhost:5000/api"),
                Timeout = TimeSpan.FromSeconds(10),
            })
            .AddSingleton<IApiService>(sp => RestService.For<IApiService>(sp.GetRequiredService<HttpClient>()))
            // Repositories
            .AddSingleton<OrdersRepository>()
            .AddSingleton<IncomesRepository>()
            .AddSingleton<PaymentsRepository>()
            // View Models
            .AddTransient<Presentation.Main.MainViewModel>()
            .AddTransient<Presentation.OrdersList.OrdersListViewModel>()
            .AddTransient<Presentation.OrderDetails.OrderDetailsViewModel>()
            .AddTransient<Presentation.IncomesList.IncomesListViewModel>()
            .AddTransient<Presentation.IncomeSelector.IncomeSelectorViewModel>()
            // Navigation
            .AddSingleton<Func<Type, INavigationTarget>>(sp => type => (INavigationTarget)sp.GetRequiredService(type))
            .AddSingleton<Func<Type, object?, object>>(sp => (type, arg) => ActivatorUtilities.CreateInstance(sp, type, arg!))
            .AddSingleton<NavigationService>()

            .BuildServiceProvider();
        Ioc.Default.ConfigureServices(services);
    }
}
