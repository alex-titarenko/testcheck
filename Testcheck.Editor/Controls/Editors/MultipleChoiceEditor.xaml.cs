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
    /// Interaction logic for MultipleChoiceEditor.xaml
    /// </summary>
    [QuestionEditor(typeof(MultipleChoiceQuestion))]
    public partial class MultipleChoiceEditor : UserControl
    {
        #region Fields

        private MultipleChoiceQuestion _question;

        #endregion

        #region Constructors

        protected MultipleChoiceEditor()
        {
            InitializeComponent();
        }

        public MultipleChoiceEditor(MultipleChoiceQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            choicesGrid.Children.Clear();
            choicesGrid.RowDefinitions.Clear();

            for (int i = 0; i < _question.Choices.Count; i++)
            {
                choicesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                RadioButton choiceRadioButton = new RadioButton();
                choiceRadioButton.IsChecked = (i == _question.Answer);
                choiceRadioButton.Margin = new Thickness(5, 10, 5, 0);
                choiceRadioButton.ToolTip = "Set the correct answer";
                choiceRadioButton.Tag = i;
                choiceRadioButton.SetValue(Grid.RowProperty, i);
                choiceRadioButton.SetValue(Grid.ColumnProperty, 0);
                choiceRadioButton.Checked += new RoutedEventHandler(choiceButton_Checked);

                choicesGrid.Children.Add(choiceRadioButton);
                choicesGrid.Children.Add(EditorControlsHelper.CreateMoveUpButton(i, 1, (i != 0), moveUpButton_Click));
                choicesGrid.Children.Add(EditorControlsHelper.CreateMoveDownButton(i, 2, (i != _question.Choices.Count - 1), moveDownButton_Click));
                choicesGrid.Children.Add(EditorControlsHelper.CreateChoiceTextBox(i, 3, _question.Choices[i], choiceTextBox_TextChanged));
                choicesGrid.Children.Add(EditorControlsHelper.CreateRemoveChoiceButton(i, 4, removeButton_Click));
            }
        }

        #region Event Handlers

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 - 1;

            string temp = _question.Choices[key1];
            _question.Choices[key1] = _question.Choices[key2];
            _question.Choices[key2] = temp;

            if (_question.Answer == key1)
                _question.Answer = key2;
            else if (_question.Answer == key2)
                _question.Answer = key1;

            LoadQuestion();
        }

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 + 1;

            string temp = _question.Choices[key1];
            _question.Choices[key1] = _question.Choices[key2];
            _question.Choices[key2] = temp;

            if (_question.Answer == key1)
                _question.Answer = key2;
            else if (_question.Answer == key2)
                _question.Answer = key1;

            LoadQuestion();
        }

        private void choiceButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;

            if (button != null)
            {
                int answerKey = (int)button.Tag;
                _question.Answer = answerKey;
            }
        }

        private void choiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                int answerKey = (int)textBox.Tag;
                _question.Choices[answerKey] = textBox.Text;
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
                    int answerKey = (int)button.Tag;
                    _question.Choices.RemoveAt(answerKey);

                    if (answerKey == _question.Answer)
                        _question.Answer = -1;
                    else if (answerKey < _question.Answer)
                        _question.Answer--;

                    LoadQuestion();
                }
            }
        }

        private void addChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            _question.Choices.Add(String.Empty);

            LoadQuestion();
        }

        #endregion

        #endregion
    }
}
