using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace BMCWindows.Utilities
{
    public class AlignmentToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HorizontalAlignment alignment)
            {
                if (alignment == HorizontalAlignment.Left)

                {
                    return new Thickness(10, 0, 0, 0); 
                }
                else
                {
                    return new Thickness(0, 0, 10, 0); 
                }
            }
            return new Thickness(0); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
