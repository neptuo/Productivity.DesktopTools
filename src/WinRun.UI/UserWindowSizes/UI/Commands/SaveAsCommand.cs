using Neptuo;
using Neptuo.Observables.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRun.UserWindowSizes.UI.ViewModels;

namespace WinRun.UserWindowSizes.UI.Commands
{
    public class SaveAsCommand : Command
    {
        private readonly SetSizeViewModel viewModel;
        private readonly SizeRepository repository;

        public SaveAsCommand(SetSizeViewModel viewModel, SizeRepository repository)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(repository, "repository");
            this.viewModel = viewModel;
            this.repository = repository;
        }

        public override void Execute()
        {
            repository.Set(viewModel.Name, viewModel.ToSize());
        }

        public override bool CanExecute() 
            => true;
    }
}
