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
    /// Provides dynamic command registration.
    /// </summary>
    public interface ICommandCollection
    {
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
