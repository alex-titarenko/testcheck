using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TAlex.Testcheck.Core.Questions;


namespace TAlex.Testcheck.Tester.Converters
{
    public class MultipleChoiceConverter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                MultipleChoiceQuestion question = (MultipleChoiceQuestion)values[0];
                string actualAnswerString = (string)values[1];
                return question.Choices.IndexOf(actualAnswerString) == question.ActualAnswer;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
