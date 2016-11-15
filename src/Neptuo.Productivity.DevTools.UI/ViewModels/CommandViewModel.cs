using FontAwesome.WPF;
using Neptuo;
using Neptuo.Observables;
using Neptuo.Productivity.DevTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.DevTools.ViewModels
{
    public class CommandViewModel : ObservableObject, ICommandModel
    {
        private FontAwesomeIcon icon;
        public FontAwesomeIcon Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand Command { get; private set; }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChanged();
                }
            }
        }

        public CommandViewModel(FontAwesomeIcon icon, ICommand command)
        {
            Ensure.NotNull(command, "command");
            Icon = icon;
            Command = command;
        }

        public CommandViewModel(FontAwesomeIcon icon, string text, ICommand command)
        {
            Ensure.NotNull(command, "command");
            Icon = icon;
            Text = text;
            Command = command;
        }
    }
}
