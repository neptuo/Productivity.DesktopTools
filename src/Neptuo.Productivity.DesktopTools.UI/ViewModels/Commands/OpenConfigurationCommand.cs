using Neptuo.Productivity.DesktopTools.Services.StartupShortcuts;
using Neptuo.Productivity.DesktopTools.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.DesktopTools.ViewModels.Commands
{
    public class OpenConfigurationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ConfigurationViewModel viewModel = new ConfigurationViewModel(new ShortcutService("Neptuo", "Productivity", "DevTools"));

            ConfigurationWindow window = new ConfigurationWindow();
            window.ViewModel = viewModel;

            window.Show();
        }
    }
}
