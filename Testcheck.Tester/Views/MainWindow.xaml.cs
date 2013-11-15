using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TAlex.Testcheck.Tester.Controls;
using TAlex.Testcheck.Core;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Controls.Testers;
using TAlex.Testcheck.Tester.Reporting;

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
        private Dictionary<int, Question> _questions;
        private int _currQuestionIndex;

        private decimal _points;

        private int _seed = new Random().Next();

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

        void _timer_Tick(object sender, EventArgs e)
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

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_questions.Count > 0)
            {
                _points += CheckAnswer();
                progressBarTest.TotalCurrentValue += _questions[GetQuestionKeyByIndex(_currQuestionIndex)].Points;
                progressBarTest.CorrectCurrentValue = _points;

                _questions.Remove(GetQuestionKeyByIndex(_currQuestionIndex));

                _currQuestionIndex = (_currQuestionIndex >= _questions.Count) ? 0 : _currQuestionIndex;

                
                if (_questions.Count != 0)
                {
                    LoadQuestion(_currQuestionIndex);
                }
            }

            if (_questions.Count == 0)
            {
                ShowResultTest();
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_questions.Count != 0)
            {
                _currQuestionIndex--;
                if (_currQuestionIndex < 0)
                    _currQuestionIndex = _questions.Count - 1;

                LoadQuestion(_currQuestionIndex);
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_questions.Count != 0)
            {
                _currQuestionIndex++;
                if (_currQuestionIndex >= _questions.Count)
                    _currQuestionIndex = 0;

                LoadQuestion(_currQuestionIndex);
            }
        }

        #endregion

        private void LoadTest(Test test)
        {
            _test = test;
            _test.Shuffle();

            if (!String.IsNullOrEmpty(_test.Description))
            {
                if (_test.Description.Length < MaxSubjectLength)
                    Title += String.Format(" - {0} ({1})", _test.Title, _test.Description);
                else
                    Title += String.Format(" - {0} ({1}...)", _test.Title, _test.Description.Substring(0, MaxSubjectLength));
            }

            if (_test.NoTimelimit)
                timeLeftLabel.Content = "Unlimited";

            progressBarTest.Maximum = _test.TotalPoints;

            GetQuestions(_test);
            _currQuestionIndex = 0;

            if (_questions.Count != 0)
                LoadQuestion(_currQuestionIndex);

            _points = 0;

            _startTime = DateTime.Now;
            _timer.Start();
        }

        private int GetQuestionKeyByIndex(int index)
        {
            foreach (KeyValuePair<int, Question> pair in _questions)
            {
                if (index == 0) return pair.Key;
                index--;
            }

            throw new IndexOutOfRangeException();
        }

        private void GetQuestions(Test test)
        {
            _questions = new Dictionary<int, Question>();

            for (int i = 0; i < test.QuestionCount; i++)
            {
                _questions.Add(i + 1, (Question)test.Questions[i].Clone());
            }
        }

        private void LoadQuestion(int questionIndex)
        {
            Question question = _questions[GetQuestionKeyByIndex(questionIndex)];

            currentQuestionStatusBarItem.Content = String.Format("{0} of {1}", GetQuestionKeyByIndex(questionIndex), _test.QuestionCount);
            questionWebBrowser.NavigateToString(ConvertToHtml(question.Description));


            Random rnd = new Random(_seed + GetQuestionKeyByIndex(questionIndex));

            if (question is TrueFalseQuestion)
                questinChoicesScrollViewer.Content = new TrueFalseTester(question);
            else if (question is MultipleChoiceQuestion)
                questinChoicesScrollViewer.Content = new MultipleChoiceTester((MultipleChoiceQuestion)question, rnd);
            else if (question is MultipleResponseQuestion)
                questinChoicesScrollViewer.Content = new MultipleResponseTester((MultipleResponseQuestion)question, rnd);
            else if (question is EssayQuestion)
                questinChoicesScrollViewer.Content = new EssayTester(question);
            else if (question is FillBlankQuestion)
                questinChoicesScrollViewer.Content = new FillBlankTester((FillBlankQuestion)question);
            else if (question is MatchingQuestion)
                questinChoicesScrollViewer.Content = new MatchingTester((MatchingQuestion)question, rnd);
            else if (question is RankingQuestion)
                questinChoicesScrollViewer.Content = new RankingTester((RankingQuestion)question, rnd);
            else
                questinChoicesScrollViewer.Content = null;
        }

        private decimal CheckAnswer()
        {
            if (questinChoicesScrollViewer.Content is ICheckable)
                return ((ICheckable)questinChoicesScrollViewer.Content).Check();
            else
                return 0;
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

        private void ShowResultTest()
        {
            _timer.Stop();

            _testReport.AnsweredQuestion = _testReport.TotalQuestion - _questions.Count;
            _testReport.Points = _points;
            _testReport.TimeElapsed = _timeElapsed;

            ResultWindow window = new ResultWindow(_testReport);
            window.Owner = this;
            window.ShowDialog();
        }

        #endregion
    }
}
