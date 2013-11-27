using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace TAlex.Testcheck.Tester.Converters
{
    public class BooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool paramValue = bool.Parse((string)parameter);
            if (value is bool)
            {
                return (bool)value == paramValue;
            }
            else if (value is bool?)
            {
                return ((bool?)value) == paramValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool paramValue = bool.Parse((string)parameter);
            if (value is bool)
            {
                return (bool)value == paramValue;
            }
            else if (value is bool?)
            {
                return ((bool?)value) == paramValue;
            }
            return value;
        }

        #endregion
    }
}
