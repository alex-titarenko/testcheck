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
    /// Interaction logic for FillBlankEditor.xaml
    /// </summary>
    public partial class FillBlankEditor : UserControl
    {
        #region Fields

        private FillBlankQuestion _question;

        #endregion

        #region Constructors

        protected FillBlankEditor()
        {
            InitializeComponent();
        }

        public FillBlankEditor(FillBlankQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            blankTextBox.Text = _question.BlankText;
        }

        #region Event Handlers

        private void blankTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _question.BlankText = blankTextBox.Text;
        }

        #endregion

        #endregion
    }
}
