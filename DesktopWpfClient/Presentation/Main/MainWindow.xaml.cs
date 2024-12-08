using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace DesktopWpfClient.Presentation.Main;

/// <summary>
/// Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
    }
}
