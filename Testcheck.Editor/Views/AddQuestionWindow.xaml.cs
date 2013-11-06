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
using System.Windows.Shapes;

using TAlex.Testcheck.Core.Questions;

namespace TAlex.Testcheck.Editor.Views
{
    /// <summary>
    /// Interaction logic for AddQuestionWindow.xaml
    /// </summary>
    public partial class AddQuestionWindow : Window
    {
        #region Fields

        private Question _newQuestion;

        #endregion

        #region Properties

        public Question NewQuestion
        {
            get
            {
                return _newQuestion;
            }
        }

        #endregion

        #region Constructors

        public AddQuestionWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (trueFalseRadioButton.IsChecked == true)
                _newQuestion = new TrueFalseQuestion();
            else if (multipleChoiceRadioButton.IsChecked == true)
                _newQuestion = new MultipleChoiceQuestion();
            else if (multipleResponseRadioButton.IsChecked == true)
                _newQuestion = new MultipleResponseQuestion();
            else if (essayRadioButton.IsChecked == true)
                _newQuestion = new EssayQuestion();
            else if (fillBlankRadioButton.IsChecked == true)
                _newQuestion = new FillBlankQuestion();
            else if (matchingRadioButton.IsChecked == true)
                _newQuestion = new MatchingQuestion();
            else if (rankingRadioButton.IsChecked == true)
                _newQuestion = new RankingQuestion();

            if (_newQuestion == null)
            {
                MessageBox.Show(this, "Please select the type of question that you want to add.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            _newQuestion.Title = questionTitleTextBox.Text;

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #endregion
    }
}
