using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace TAlex.Testcheck.Tester.Converters
{
    public class QuestionDescriptionToHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return ConvertToHtml((string)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private string ConvertToHtml(string source)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(
                @"<html>
                    <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />

                        <style type='text/css'>
                            body { font: 14px verdana; color: #505050; background: #fcfcfc; }
                        </style>

                        <script language='JScript'>
                            function onContextMenu()
                            {
                              if (window.event.srcElement.tagName !='INPUT') 
                              {
                                window.event.returnValue = false;  
                                window.event.cancelBubble = true;
                                return false;
                              }
                            }

                            function onLoad()
                            {
                              document.oncontextmenu = onContextMenu; 
                            }
                        </script>
                    </head>
                    <body onload='onLoad();'>"
                );

            sb.Append(source);

            sb.Append(
                    @"</body>
                </html>"
                );

            return sb.ToString();
        }
    }
}
