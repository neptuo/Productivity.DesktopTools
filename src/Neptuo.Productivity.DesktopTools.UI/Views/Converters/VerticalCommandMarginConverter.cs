using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Neptuo.Productivity.DesktopTools.Views.Converters
{
    public class VerticalCommandMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VerticalOrientation vertical = (VerticalOrientation)value;
            if (vertical == VerticalOrientation.Top)
                return new Thickness(10, 10, 10, 0);

            return new Thickness(10, 0, 10, 10);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
