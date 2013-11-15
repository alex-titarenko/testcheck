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
    /// Interaction logic for MultipleResponseTester.xaml
    /// </summary>
    public partial class MultipleResponseTester : UserControl, ICheckable
    {
        #region Fields

        private Question Question;

        #endregion

        #region Constructors

        protected MultipleResponseTester()
        {
            InitializeComponent();
        }

        public MultipleResponseTester(Question question, Random rand)
            : this()
        {
            //int[] indexes = TAlex.Testcheck.Core.Helpers.Shuffles.GetRandomSequence(Question.Choices.Count, Question.ShuffleMode, rand);
            DataContext = Question = question;
        }

        #endregion

        #region Methods

        public decimal Check()
        {
            return Question.Check("");
        }

        #endregion
    }
}
