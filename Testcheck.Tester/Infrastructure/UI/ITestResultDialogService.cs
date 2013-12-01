using System;
using TAlex.Testcheck.Tester.Reporting;


namespace TAlex.Testcheck.Tester.Infrastructure.UI
{
    public interface ITestResultDialogService
    {
        void ShowResult(TestReport report);
    }
}
