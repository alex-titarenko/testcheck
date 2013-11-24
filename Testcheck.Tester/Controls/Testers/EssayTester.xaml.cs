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
    public partial class EssayTester : UserControl
    {
        #region Fields

        protected Question Question;

        #endregion

        #region Constructors

        protected EssayTester()
        {
            InitializeComponent();
        }

        public EssayTester(Question question)
            : this()
        {
            DataContext = Question = question;
        }

        #endregion
    }
}
