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
    /// Interaction logic for EssayTester.xaml
    /// </summary>
    public partial class EssayTester : UserControl, ICheckable
    {
        #region Fields

        private EssayQuestion _question;

        #endregion

        #region Constructors

        protected EssayTester()
        {
            InitializeComponent();
        }

        public EssayTester(EssayQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            choiceTextBox.Text = String.Empty;
        }

        public decimal Check()
        {
            string text = choiceTextBox.Text.Trim();

            //if (String.IsNullOrEmpty(choiceTextBox.Text))
            //    throw new Exception();

            return _question.Check(text);
        }

        #endregion
    }
}
