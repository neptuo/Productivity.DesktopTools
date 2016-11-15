using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Neptuo.Productivity.DevTools.Views.Converters
{
    public class HorizontalDockConverter : IValueConverter
    {
        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HorizontalOrientation orientation = (HorizontalOrientation)value;
            if (orientation == HorizontalOrientation.Left)
            {
                if (IsInverted)
                    return Dock.Right;

                return Dock.Left;
            }

            if (IsInverted)
                return Dock.Left;

            return Dock.Right;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
