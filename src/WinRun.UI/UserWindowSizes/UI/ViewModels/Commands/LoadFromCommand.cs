using Neptuo;
using Neptuo.Observables.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UserWindowSizes.UI.ViewModels.Commands
{
    public class LoadFromCommand : Command<string>
    {
        private readonly SizeRepository repository;
        private readonly SetSizeViewModel viewModel;

        public LoadFromCommand(SetSizeViewModel viewModel, SizeRepository repository)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(repository, "repository");
            this.viewModel = viewModel;
            this.repository = repository;
        }

        public override bool CanExecute(string parameter)
            => !String.IsNullOrEmpty(parameter);

        public override void Execute(string parameter)
        {
            SizeModel model = repository.Find(parameter);
            if (model != null)
            {
                viewModel.Left = model.Left;
                viewModel.Top = model.Top;
                viewModel.Width = model.Width;
                viewModel.Height = model.Height;
            }
        }
    }
}
