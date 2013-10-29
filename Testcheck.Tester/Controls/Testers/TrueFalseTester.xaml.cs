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
    /// Interaction logic for TrueFalseTester.xaml
    /// </summary>
    public partial class TrueFalseTester : UserControl, ICheckable
    {
        #region Fields

        private TrueFalseQuestion _question;

        #endregion

        #region Constructors

        protected TrueFalseTester()
        {
            InitializeComponent();
        }

        public TrueFalseTester(TrueFalseQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {

        }

        public decimal Check()
        {
            bool? choice = null;

            if (trueRadioButton.IsChecked == true)
                choice = true;
            else if (falseRadioButton.IsChecked == true)
                choice = false;

            if (choice != null)
                return _question.Check((bool)choice);
            else
                return 0;
        }

        #endregion
    }
}
