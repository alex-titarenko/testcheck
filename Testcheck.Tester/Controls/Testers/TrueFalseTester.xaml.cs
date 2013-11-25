using System;
using System.Windows.Controls;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for TrueFalseTester.xaml
    /// </summary>
    [QuestionTester(typeof(TrueFalseQuestion))]
    public partial class TrueFalseTester : UserControl
    {
        #region Constructors

        protected TrueFalseTester()
        {
            InitializeComponent();
        }

        public TrueFalseTester(TrueFalseQuestion question)
            : this()
        {
            DataContext = question;
        }

        #endregion
    }
}
