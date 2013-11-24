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

using TAlex.Testcheck.Core.Questions;

namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for MultipleChoiceTester.xaml
    /// </summary>
    public partial class MultipleChoiceTester : UserControl, ICheckable
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

        public decimal Check()
        {
            return Question.Check("");
        }

        #endregion
    }
}
