using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools
{
    /// <summary>
    /// A component with short and longer description.
    /// </summary>
    public interface ITextModel
    {
        /// <summary>
        /// Gets the component label.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the additional component description.
        /// </summary>
        string Description { get; }
    }
}
