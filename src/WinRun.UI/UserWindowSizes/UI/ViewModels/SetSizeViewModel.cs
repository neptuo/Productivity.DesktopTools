using Neptuo;
using Neptuo.Observables;
using Neptuo.Observables.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRun.UserWindowSizes.UI.Commands;
using WinRun.UserWindowSizes.UI.ViewModels.Commands;

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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
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
        private readonly SizeRepository repository;

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

        public ObservableCollection<string> Names { get; }

        public ICommand Apply { get; }
        public SaveAsCommand SaveAs { get; }
        public LoadFromCommand LoadFrom { get; }

        public SetSizeViewModel(string title, IWindowManager manager, SizeRepository repository)
        {
            this.repository = repository;

            Name = title;
            Names = new ObservableCollection<string>();
            Apply = new ApplyCommand(this, manager);
            SaveAs = new SaveAsCommand(this, repository);
            LoadFrom = new LoadFromCommand(this, repository);

            ReloadNames();
        }

        public void ReloadNames()
        {
            Names.Clear();
            Names.AddRange(repository.GetNames().OrderBy(n => n));
        }

        public SizeModel ToSize() => new SizeModel()
        {
            Left = Left,
            Top = Top,
            Width = Width,
            Height = Height
        };
    }
}
