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
using System.Windows.Shapes;

using TAlex.Testcheck.Tester.Reporting;

namespace TAlex.Testcheck.Tester.Views
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        #region Constructors

        public ResultWindow(TestReport testReport)
        {
            InitializeComponent();

            userNameLabel.Content = testReport.UserName;
            userGroupLabel.Content = testReport.UserGroup;

            totalQuestionsLabel.Content = testReport.TotalQuestion;
            answeredQuestionLabel.Content = testReport.AnsweredQuestion;

            pointsLabel.Content = testReport.Points.ToString("N2");

            decimal percentCorrect = testReport.PercentCorrect;
            percentCorrectLabel.Content = percentCorrect.ToString("P");

            TimeSpan timeElapsed = testReport.TimeElapsed;
            timeElapsedLabel.Content = String.Format("{0:D2}:{1:D2}:{2:D2}", timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);

            gradeLabel.Content = testReport.Grade.ToString("N2");

        }

        #endregion

        #region Methods

        #region Event Handlers

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        #endregion
    }
}
