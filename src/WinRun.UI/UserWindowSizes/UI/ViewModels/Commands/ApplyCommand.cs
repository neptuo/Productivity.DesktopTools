using Neptuo;
using Neptuo.Observables.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinRun.UserWindowSizes.UI.ViewModels.Commands
{
    public class ApplyCommand : Command
    {
        private readonly SetSizeViewModel viewModel;
        private readonly IWindowManager manager;

        public ApplyCommand(SetSizeViewModel viewModel, IWindowManager manager)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(manager, "manager");
            this.viewModel = viewModel;
            this.manager = manager;
        }

        public override void Execute() 
            => manager.Update(viewModel.Left, viewModel.Top, viewModel.Width, viewModel.Height, viewModel.IsCurrentMonitor);

        public override bool CanExecute() 
            => true;
    }
}
