using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel.DataAnnotations;


namespace TAlex.Testcheck.Editor.Converters
{
    public class EnumValueToReadableValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType().IsEnum)
            {
                Enum enumVal = (Enum)value;
                DisplayAttribute displayAttribute = value.GetType().GetField(value.ToString()).GetCustomAttribute<DisplayAttribute>();
                return (displayAttribute != null && !String.IsNullOrEmpty(displayAttribute.Name)) ? displayAttribute.Name : enumVal.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
