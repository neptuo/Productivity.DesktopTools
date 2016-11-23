using Neptuo;
using Neptuo.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.DevTools.ViewModels.Commands
{
    internal class TryWriteToLogCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;


        private readonly ILog log;

        public TryWriteToLogCommand(ILogFactory logFactory)
        {
            Ensure.NotNull(logFactory, "logFactory");
            this.log = logFactory.Scope("TryWriteToLog");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            log.Warning($"Hello, World at {DateTime.Now}");
        }
    }
}
