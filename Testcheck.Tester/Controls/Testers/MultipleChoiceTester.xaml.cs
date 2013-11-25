using System;
using System.Windows;
using System.Windows.Controls;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for MultipleChoiceTester.xaml
    /// </summary>
    [QuestionTester(typeof(MultipleChoiceQuestion))]
    public partial class MultipleChoiceTester : UserControl
    {
        #region Fields

        protected MultipleChoiceQuestion Question;

        #endregion

        #region Constructors

        protected MultipleChoiceTester()
        {
            InitializeComponent();
        }

        public MultipleChoiceTester(MultipleChoiceQuestion question)
            : this()
        {
            DataContext = Question = question;
        }

        #endregion

        #region Methods

        private void answerChoiceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            string answer = ((FrameworkElement)sender).DataContext as String;
            Question.ActualAnswer = Question.Choices.IndexOf(answer);
        }

        #endregion
    }
}
