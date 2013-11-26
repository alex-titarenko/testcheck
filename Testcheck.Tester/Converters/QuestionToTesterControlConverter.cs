using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Converters
{
    public class QuestionToTesterControlConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Question question = value as Question;
            if (question != null)
            {
                Type testerType = typeof(App).Assembly.DefinedTypes
                    .FirstOrDefault(x => x.GetCustomAttributes<QuestionTesterAttribute>()
                        .Any(a => a.QuestionType == question.GetType()));

                if (testerType != null)
                {
                    return Activator.CreateInstance(testerType, question);
                }
                return null;
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
