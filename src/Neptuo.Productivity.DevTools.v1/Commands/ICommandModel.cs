using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.DevTools.Commands
{
    /// <summary>
    /// A model describing command.
    /// </summary>
    public interface ICommandModel
    {
        /// <summary>
        /// Gets the icon of command.
        /// </summary>
        FontAwesomeIcon Icon { get; }

        /// <summary>
        /// Gets the additional commad label.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the function to execute.
        /// </summary>
        ICommand Command { get; }

        /// <summary>
        /// Gets the additional command description.
        /// </summary>
        string Description { get; }
    }
}
