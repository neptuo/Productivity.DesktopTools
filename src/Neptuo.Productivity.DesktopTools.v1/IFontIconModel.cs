using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools
{
    /// <summary>
    /// A component with icon.
    /// </summary>
    public interface IFontIconModel
    {
        /// <summary>
        /// Gets the icon of component.
        /// </summary>
        FontAwesomeIcon Icon { get; }
    }
}
