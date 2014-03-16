using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TAlex.Testcheck.Core.Helpers
{
    public class QuestionContentHelper
    {
        public static string WrapHtmlContent(string source)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(
                @"<html>
                    <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />

                        <style type='text/css'>
                            body { font: 10pt verdana; color: #505050; background: #fcfcfc; }
                            table, td, th, tr { border: 1px solid black; border-collapse: collapse; }
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
