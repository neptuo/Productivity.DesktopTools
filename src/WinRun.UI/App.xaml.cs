using System.Windows;

namespace WinRun.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Application.Current.MainWindow = window;
        }
    }
}
