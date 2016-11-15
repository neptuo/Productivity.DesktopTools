using Neptuo.Observables.Collections;
using Neptuo.Productivity.DevTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontAwesome.WPF;
using System.Windows.Input;
using Neptuo;

namespace Neptuo.Productivity.DevTools.ViewModels
{
    internal class CommandObservableCollection : ObservableCollection<ICommandModel>, ICommandCollection
    {
        ICommandCollection ICommandCollection.Add(FontAwesomeIcon icon, string text, ICommand command, string description)
        {
            Add(new CommandViewModel(icon, text, command) { Description = description });
            return this;
        }

        IEnumerable<ICommandModel> ICommandCollection.Enumerate()
        {
            return this;
        }

        ICommandCollection ICommandCollection.Add(ICommandModel command)
        {
            Ensure.NotNull(command, "command");
            Add(command);
            return this;
        }
    }
}
