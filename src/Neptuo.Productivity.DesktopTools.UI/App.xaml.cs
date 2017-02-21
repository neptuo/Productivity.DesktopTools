using FontAwesome.WPF;
using Neptuo.Activators;
using Neptuo.Logging;
using Neptuo.Productivity.DesktopTools.Commands;
using Neptuo.Productivity.DesktopTools.ViewModels;
using Neptuo.Productivity.DesktopTools.ViewModels.Commands;
using Neptuo.Productivity.DesktopTools.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Neptuo.Productivity.DesktopTools
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

            MainViewModel viewModel = new MainViewModel();
            viewModel.Settings.MainBorder.Size = 60;
            viewModel.Settings.CommandBorder.Size = 40;

            DefaultLogFactory logFactory = new DefaultLogFactory("root");
            logFactory.AddSerializer(viewModel.Message);

            dependencyContainer.Definitions
                .AddScoped<ICommandCollection>(dependencyContainer.ScopeName, viewModel.Commands)
                .AddScoped<ILogFactory>(dependencyContainer.ScopeName, logFactory);

            RegisterCommands(dependencyContainer, viewModel.Commands);

            MainWindow view = new MainWindow();
            view.ViewModel = viewModel;
            view.Show();
        }

        private void RegisterCommands(IDependencyContainer dependencyContainer, ICommandCollection commands)
        {
            commands
                .Add(new CommandViewModel(FontAwesomeIcon.Cog, "Configuration", new OpenConfigurationCommand()))
                .Add(new CommandViewModel(FontAwesomeIcon.Try, "Try write to log", new TryWriteToLogCommand(dependencyContainer.Resolve<ILogFactory>())));
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
