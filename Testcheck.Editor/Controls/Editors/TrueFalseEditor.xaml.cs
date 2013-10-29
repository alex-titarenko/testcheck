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

namespace TAlex.Testcheck.Editor.Controls.Editors
{
    /// <summary>
    /// Interaction logic for TrueFalseEditor.xaml
    /// </summary>
    public partial class TrueFalseEditor : UserControl
    {
        #region Fields

        private TrueFalseQuestion _question;

        #endregion

        #region Constructors

        protected TrueFalseEditor()
        {
            InitializeComponent();
        }

        public TrueFalseEditor(TrueFalseQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            if (_question.Correctly)
                trueRadioButton.IsChecked = true;
            else
                falseRadioButton.IsChecked = true;
        }

        #region Event Handlers

        private void trueRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _question.Correctly = true;
        }

        private void falseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _question.Correctly = false;
        }

        #endregion

        #endregion
    }
}
