using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Neptuo.Productivity.DesktopTools.Views.Converters
{
    public class VerticalDockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VerticalOrientation orientation = (VerticalOrientation)value;
            if (orientation == VerticalOrientation.Top)
                return Dock.Top;

            return Dock.Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
