using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TAlex.Testcheck.Core;
using TAlex.Testcheck.Tester.Controls;
using TAlex.Testcheck.Tester.Controls.Testers;
using TAlex.Testcheck.Tester.Reporting;
using TAlex.Testcheck.Tester.Infrastructure;
using TAlex.Testcheck.Tester.ViewModels;


namespace TAlex.Testcheck.Tester.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private const int MaxSubjectLength = 60;

        private System.Windows.Threading.DispatcherTimer _timer;
        private TimeSpan _timeElapsed;
        private DateTime _startTime;
        
        private Test _test;
        private TestReport _testReport;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.2);
            _timer.Tick += new EventHandler(_timer_Tick);
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            Test test = null;

            if (args.Length == 2)
            {
                try
                {
                    test = Test.Load(args[1]);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, String.Format(Properties.Resources.CanNotLoadFile, args[1]),
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    Application.Current.Shutdown();
                }
            }
            else
            {
                ChoiceTestWindow window = new ChoiceTestWindow();
                window.Owner = this;

                if (window.ShowDialog() == true && window.DialogResult == true)
                {
                    test = window.SelectedTest;
                }
            }

            // User authorization
            UserAuthorizationWindow authWindow = new UserAuthorizationWindow();
            authWindow.Owner = this;
            if (authWindow.ShowDialog() == true)
            {
                _testReport = new TestReport();

                _testReport.UserName = authWindow.UserName;
                _testReport.UserGroup = authWindow.UserGroup;
                _testReport.TotalQuestion = test.QuestionCount;
                _testReport.GradingScale = test.GradingScale;
                _testReport.TotalPoints = test.TotalPoints;
            }

            // Load and run the test
            LoadTest(test);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Do you really want to stop testing? All results will be lost.", "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timeElapsed = DateTime.Now - _startTime;
            TimeSpan timeLeft = _test.Timelimit - _timeElapsed;

            if (!_test.NoTimelimit && timeLeft <= TimeSpan.Zero)
            {
                ShowResultTest();
            }
            else
            {
                timeElapsedLabel.Content = String.Format("{0:D2}:{1:D2}:{2:D2}", _timeElapsed.Hours, _timeElapsed.Minutes, _timeElapsed.Seconds);

                if (_test.NoTimelimit == false)
                {
                    if (timeLeft <= TimeSpan.FromMilliseconds(_test.Timelimit.TotalMilliseconds * 0.1))
                        timeLeftLabel.Foreground = Brushes.Red;

                    timeLeftLabel.Content = String.Format("{0:D2}:{1:D2}:{2:D2}", timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
                }
            }
        }

        private void questionWebBrowser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                case Key.BrowserRefresh:
                case Key.Apps:
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        private void LoadTest(Test test)
        {
            _test = test;
            test.Shuffle();

            if (!String.IsNullOrEmpty(test.Description))
            {
                if (test.Description.Length < MaxSubjectLength)
                    Title += String.Format(" - {0} ({1})", test.Title, test.Description);
                else
                    Title += String.Format(" - {0} ({1}...)", test.Title, test.Description.Substring(0, MaxSubjectLength));
            }

            if (test.NoTimelimit)
                timeLeftLabel.Content = "Unlimited";

            DataContext = new TesterViewModel(test);

            _startTime = DateTime.Now;
            _timer.Start();
        }

        

        private void ShowResultTest()
        {
            _timer.Stop();

            //_testReport.AnsweredQuestion = _testReport.TotalQuestion - _questions.Count; // TODO:
            //_testReport.Points = _points; //TODO:
            _testReport.TimeElapsed = _timeElapsed;

            ResultWindow window = new ResultWindow(_testReport);
            window.Owner = this;
            window.ShowDialog();
        }

        #endregion
    }
}
