using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAlex.Testcheck.Tester.Reporting;
using TAlex.Testcheck.Tester.Views;
using TAlex.WPF.Mvvm.Extensions;


namespace TAlex.Testcheck.Tester.Infrastructure.UI
{
    public class TestResultDialogService : ITestResultDialogService
    {
        #region ITestResultDialogService Members

        public void ShowResult(TestReport report)
        {
            ResultWindow window = new ResultWindow(report) { Owner = App.Current.GetActiveWindow() };
            window.ShowDialog();
        }

        #endregion
    }
}
