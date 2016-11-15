using Neptuo.Activators;
using Neptuo.Productivity.DevTools.Commands;
using Neptuo.Productivity.DevTools.ViewModels;
using Neptuo.Productivity.DevTools.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Neptuo.Productivity.DevTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDependencyContainer dependencyContainer = new UnityDependencyContainer();

            MainViewModel viewModel = new Views.DesignData.ViewModelLocator().Main;
            dependencyContainer.Definitions
                .AddScoped<ICommandCollection>(dependencyContainer.ScopeName, viewModel.Commands);

            MainWindow view = new MainWindow();
            view.ViewModel = viewModel;
            view.Show();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            StringBuilder message = new StringBuilder();

            string exceptionMessage = e.Exception.ToString();
            if (exceptionMessage.Length > 800)
                exceptionMessage = exceptionMessage.Substring(0, 800);

            message.AppendLine(exceptionMessage);

            MessageBoxResult result = MessageBox.Show(message.ToString(), "Do you want to kill the aplication?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                Shutdown();

            e.Handled = true;
        }
    }
}
