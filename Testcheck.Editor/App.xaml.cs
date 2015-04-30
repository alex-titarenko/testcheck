using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using TAlex.Common.Diagnostics.ErrorReporting;
using TAlex.Common.Models;
using TAlex.Testcheck.Editor.Locators;
using TAlex.Testcheck.Editor.Services.Licensing;
using TAlex.Testcheck.Editor.Views;


namespace TAlex.Testcheck.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Properties

        internal AppLicense License
        {
            get
            {
                ViewModelLocator locator = Resources["viewModelLocator"] as ViewModelLocator;
                return locator.Get<AppLicense>();
            }
        }

        #endregion

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
            // check license
            CheckTrialExpiration();
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

        private void CheckTrialExpiration()
        {
            AppLicense license = License;

            if (license.IsTrial && license.TrialHasExpired)
            {
                if (MessageBox.Show(TAlex.Testcheck.Editor.Properties.Resources.locEvaluationPeriodHasExpired,
                    TAlex.Testcheck.Editor.Properties.Resources.locInformationMessageCaption,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    RegistrationWindow window = new RegistrationWindow();
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                else
                {
                    Shutdown();
                }
            }
        }

        #endregion
    }
}
