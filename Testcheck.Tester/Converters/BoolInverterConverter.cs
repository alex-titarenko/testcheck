using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace TAlex.Testcheck.Tester.Converters
{
    public class BoolInverterConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            else if (value is bool?)
            {
                return ((bool?)value).HasValue ? !((bool?)value).Value : (bool?)null;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            else if (value is bool?)
            {
                return ((bool?)value).HasValue ? !((bool?)value).Value : (bool?)null;
            }
            return value;
        }

        #endregion
    }
}
