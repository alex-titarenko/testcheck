using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;


namespace TAlex.Testcheck.Tester.Converters
{
    public class TimeLeftToBrushConverter : IMultiValueConverter
    {
        #region Fields

        private Brush _defaultBrush;

        #endregion

        #region Constructors

        public TimeLeftToBrushConverter()
        {
            _defaultBrush = Brushes.DimGray;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is TimeSpan?)
            {
                TimeSpan? timeLimit = (TimeSpan?)values[0];
                TimeSpan? timeLeft = (TimeSpan?)values[1];

                if (timeLimit.HasValue)
                {
                    if (timeLeft <= TimeSpan.FromMilliseconds(timeLimit.Value.TotalMilliseconds * 0.1))
                        return Brushes.Red;
                }
                return _defaultBrush;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
