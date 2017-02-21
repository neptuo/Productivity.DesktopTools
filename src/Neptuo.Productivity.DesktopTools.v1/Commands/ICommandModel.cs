using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.DesktopTools.Commands
{
    /// <summary>
    /// A model describing command.
    /// </summary>
    public interface ICommandModel
    {
        /// <summary>
        /// Gets the function to execute.
        /// </summary>
        ICommand Command { get; }
    }
}
