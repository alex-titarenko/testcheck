using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace TAlex.Testcheck.Tester.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        #region Fields

        private static readonly string UnsetValue = "Unlimited";

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan || value is TimeSpan?)
            {
                TimeSpan? timeSpan = (value is TimeSpan) ? (TimeSpan)value : (TimeSpan?)value;
                if (timeSpan.HasValue)
                    return String.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Value.Hours, timeSpan.Value.Minutes, timeSpan.Value.Seconds);
                else
                    return UnsetValue;
            }
            return UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
