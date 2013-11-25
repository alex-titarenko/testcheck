using System;
using System.Windows.Controls;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for MultipleResponseTester.xaml
    /// </summary>
    [QuestionTester(typeof(MultipleResponseQuestion))]
    public partial class MultipleResponseTester : UserControl
    {
        #region Constructors

        protected MultipleResponseTester()
        {
            InitializeComponent();
        }

        public MultipleResponseTester(MultipleResponseQuestion question)
            : this()
        {
            DataContext = question;
        }

        #endregion
    }
}
