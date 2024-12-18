using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Trace.Common.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null&&value is string)
            {
                string? result=value as string;
                if (result=="true")
                    return 1;
                else if (result == "false")
                    return 0;
            }
            return -1;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is int)
            {
                int intValue = (int)value;
                if (intValue == 1)
                    return "true";
                else if (intValue == 0)
                    return "false";
            }
            return ""; 
        }
    }
}
