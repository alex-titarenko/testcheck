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
using TAlex.Testcheck.Editor.Infrastructure;

namespace TAlex.Testcheck.Editor.Controls.Editors
{
    /// <summary>
    /// Interaction logic for EssayEditor.xaml
    /// </summary>
    [QuestionEditor(typeof(EssayQuestion))]
    public partial class EssayEditor : UserControl
    {
        #region Fields

        private EssayQuestion _question;

        #endregion

        #region Constructors

        protected EssayEditor()
        {
            InitializeComponent();
        }

        public EssayEditor(EssayQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            correctAnswersGrid.Children.Clear();
            correctAnswersGrid.RowDefinitions.Clear();

            for (int i = 0; i < _question.CorrectAnswers.Count; i++)
            {
                correctAnswersGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                correctAnswersGrid.Children.Add(EditorControlsHelper.CreateChoiceTextBox(i, 0, _question.CorrectAnswers[i], choiceTextBox_TextChanged));
                correctAnswersGrid.Children.Add(EditorControlsHelper.CreateRemoveChoiceButton(i, 1, removeButton_Click));
            }
        }

        #region Event Handlers

        private void choiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                int answerIndex = (int)textBox.Tag;
                _question.CorrectAnswers[answerIndex] = textBox.Text;
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                if (MessageBox.Show(Window.GetWindow(this), Properties.Resources.RemoveChoiceQuestion, "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int answerIndex = (int)button.Tag;

                    _question.CorrectAnswers.RemoveAt(answerIndex);
                    LoadQuestion();
                }
            }
        }

        private void addCorrectAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            _question.CorrectAnswers.Add(String.Empty);
            LoadQuestion();
        }

        #endregion

        #endregion
    }
}
