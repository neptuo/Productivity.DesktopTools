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
    /// Provides dynamic command registration.
    /// </summary>
    public interface ICommandCollection
    {
        /// <summary>
        /// Registers a new command.
        /// </summary>
        /// <param name="icon">The icon for command.</param>
        /// <param name="text">The additional command label.</param>
        /// <param name="command">The function to execute.</param>
        /// <param name="description">The additional command description.</param>
        /// <returns>Self (for fluency).</returns>
        ICommandCollection Add(FontAwesomeIcon icon, string text, ICommand command, string description);

        /// <summary>
        /// Registers a new command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <returns>Self (for fluency).</returns>
        ICommandCollection Add(ICommandModel command);

        /// <summary>
        /// Enumerates all registered commands.
        /// </summary>
        /// <returns>Enumeration of all registered commands.</returns>
        IEnumerable<ICommandModel> Enumerate();
    }
}
