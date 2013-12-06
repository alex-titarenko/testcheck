using System;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Editor.Infrastructure;


namespace TAlex.Testcheck.Editor.Converters
{
    public class QuestionToEditorControlConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Question question = value as Question;
            if (question != null)
            {
                Type editorType = typeof(App).Assembly.DefinedTypes
                    .FirstOrDefault(x => x.GetCustomAttributes<QuestionEditorAttribute>()
                        .Any(a => a.QuestionType == question.GetType()));

                if (editorType != null)
                {
                    return Activator.CreateInstance(editorType, question);
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
