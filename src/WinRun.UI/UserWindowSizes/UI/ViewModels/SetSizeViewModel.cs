using Neptuo;
using Neptuo.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinRun.UserWindowSizes.UI.ViewModels
{
    public class SetSizeViewModel : ObservableModel
    {
        private bool isCurrentMonitor;
        public bool IsCurrentMonitor
        {
            get { return isCurrentMonitor; }
            set
            {
                if (isCurrentMonitor != value)
                {
                    isCurrentMonitor = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int left;
        public int Left
        {
            get { return left; }
            set
            {
                if (left != value)
                {
                    left = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int top;
        public int Top
        {
            get { return top; }
            set
            {
                if (top != value)
                {
                    top = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand ApplyCommand { get; private set; }

        public SetSizeViewModel(string title, IWindowManager manager)
        {
            Title = title;
            ApplyCommand = new ApplyCommandImpl(this, manager);
        }

        public interface IWindowManager
        {
            void Update(int left, int top, int width, int height, bool isCurrentMonitor);
        }

        private class ApplyCommandImpl : ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            private readonly SetSizeViewModel viewModel;
            private readonly IWindowManager manager;

            public ApplyCommandImpl(SetSizeViewModel viewModel, IWindowManager manager)
            {
                Ensure.NotNull(viewModel, "viewModel");
                Ensure.NotNull(manager, "manager");
                this.viewModel = viewModel;
                this.manager = manager;
            }

            public void Execute(object parameter)
            {
                manager.Update(viewModel.left, viewModel.Top, viewModel.width, viewModel.Height, viewModel.IsCurrentMonitor);
            }
        }
    }
}
