using System;
using System.Windows.Controls;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for EssayTester.xaml
    /// </summary>
    [QuestionTester(typeof(EssayQuestion))]
    public partial class EssayTester : UserControl
    {
        #region Constructors

        protected EssayTester()
        {
            InitializeComponent();
        }

        public EssayTester(EssayQuestion question)
            : this()
        {
            DataContext = question;
        }

        #endregion
    }
}
