using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Trace.Common.Converters
{
    public class IntToRoleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string roleName)
            {
                var roles = new[] { "Administrator", "Engineer", "Worker" }; // 定义角色列表
                return Array.IndexOf(roles, roleName);
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && index >= 0)
            {
                var roles = new[] {   "Administrator","Engineer","Worker" }; // 定义角色列表
                if (index < roles.Length)
                {
                    return roles[index];
                }
            }
            return Binding.DoNothing;
        }
    }
}
