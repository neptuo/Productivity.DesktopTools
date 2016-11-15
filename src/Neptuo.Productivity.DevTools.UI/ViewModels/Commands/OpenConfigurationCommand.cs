using Neptuo.Productivity.DevTools.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.DevTools.ViewModels.Commands
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
            ConfigurationWindow window = new ConfigurationWindow();
            window.Show();
        }
    }
}
