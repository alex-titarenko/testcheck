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
    public partial class TrueFalseTester : UserControl
    {
        #region Constructors

        protected TrueFalseTester()
        {
            InitializeComponent();
        }

        public TrueFalseTester(Question question)
            : this()
        {
            DataContext = question;
        }

        #endregion
    }
}
