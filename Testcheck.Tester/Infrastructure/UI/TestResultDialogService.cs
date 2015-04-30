using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TAlex.Testcheck.Tester.Reporting;
using TAlex.Testcheck.Tester.Views;
using TAlex.Mvvm.Extensions;


namespace TAlex.Testcheck.Tester.Infrastructure.UI
{
    public class TestResultDialogService : ITestResultDialogService
    {
        #region ITestResultDialogService Members

        public void ShowResult(TestReport report)
        {
            Window activeWindow = App.Current.GetActiveWindow();

            activeWindow.Dispatcher.Invoke(() =>
            {
                ResultWindow window = new ResultWindow(report) { Owner = activeWindow };
                window.ShowDialog();
            });
        }

        #endregion
    }
}
