using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using TAlex.Common.Diagnostics.Reporting;
using TAlex.Common.Models;
using TAlex.Testcheck.Editor.Views;


namespace TAlex.Testcheck.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructors

        public App()
        {
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        #endregion

        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ProcessingUnhandledException(e.Exception);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ProcessingUnhandledException(e.ExceptionObject as Exception);
        }

        private void ProcessingUnhandledException(Exception exc)
        {
            Trace.TraceError(exc.ToString());

            ErrorReportingWindow reportWindow =
                new ErrorReportingWindow(new ErrorReport(exc), new AssemblyInfo(Assembly.GetEntryAssembly()));

            Window activeWindow = null;
            foreach (Window w in Windows)
            {
                if (w.IsActive)
                {
                    activeWindow = w;
                    break;
                }
            }

            if (activeWindow != null)
            {
                reportWindow.Owner = activeWindow;
            }
            reportWindow.ShowDialog();
        }

        #endregion
    }
}
