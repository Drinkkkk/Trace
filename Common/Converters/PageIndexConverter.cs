using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Trace.Common.Converters
{
    public class PageIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int pageIndex)
            {
                return pageIndex + 1; // 从0基转换为1基
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int displayPageIndex)
            {
                return displayPageIndex - 1; // 从1基转换回0基
            }
            return 0;
        }
    }
}
