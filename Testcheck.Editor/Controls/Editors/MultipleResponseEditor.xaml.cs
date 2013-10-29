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

using TAlex.Testcheck.Tester.TestCore.Choices;
using TAlex.Testcheck.Tester.TestCore.Questions;

namespace TAlex.Testcheck.Editor.Controls.Editors
{
    /// <summary>
    /// Interaction logic for MultipleResponseEditor.xaml
    /// </summary>
    public partial class MultipleResponseEditor : UserControl
    {
        #region Fields

        private MultipleResponseQuestion _question;

        #endregion

        #region Constructors

        protected MultipleResponseEditor()
        {
            InitializeComponent();
        }

        public MultipleResponseEditor(MultipleResponseQuestion question)
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

                CheckBox choiceCheckBox = new CheckBox();
                choiceCheckBox.IsChecked = _question.Choices[i].IsCorrect;
                choiceCheckBox.Margin = new Thickness(5, 10, 5, 0);
                choiceCheckBox.ToolTip = "Set the correct answer";
                choiceCheckBox.Tag = i;
                choiceCheckBox.SetValue(Grid.RowProperty, i);
                choiceCheckBox.SetValue(Grid.ColumnProperty, 0);
                choiceCheckBox.Checked += new RoutedEventHandler(choiceButton_Checked);
                choiceCheckBox.Unchecked += new RoutedEventHandler(choiceButton_Unchecked);

                choicesGrid.Children.Add(choiceCheckBox);
                choicesGrid.Children.Add(EditorControlsHelper.CreateMoveUpButton(i, 1, (i != 0), moveUpButton_Click));
                choicesGrid.Children.Add(EditorControlsHelper.CreateMoveDownButton(i, 2, (i != _question.Choices.Count - 1), moveDownButton_Click));
                choicesGrid.Children.Add(EditorControlsHelper.CreateChoiceTextBox(i, 3, _question.Choices[i].Description, choiceTextBox_TextChanged));
                choicesGrid.Children.Add(EditorControlsHelper.CreateRemoveChoiceButton(i, 4, removeButton_Click));
            }
        }

        #region Event Handlers

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 - 1;

            AnswerChoice temp = _question.Choices[key1];
            _question.Choices[key1] = _question.Choices[key2];
            _question.Choices[key2] = temp;

            LoadQuestion();
        }

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 + 1;

            AnswerChoice temp = _question.Choices[key1];
            _question.Choices[key1] = _question.Choices[key2];
            _question.Choices[key2] = temp;

            LoadQuestion();
        }

        private void choiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                int answerIndex = (int)textBox.Tag;
                _question.Choices[answerIndex].Description = textBox.Text;
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

                    _question.Choices.RemoveAt(answerIndex);
                    LoadQuestion();
                }
            }
        }

        private void addChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            _question.Choices.Add(new AnswerChoice(String.Empty, false));
            LoadQuestion();
        }

        private void choiceButton_Checked(object sender, RoutedEventArgs e)
        {
            ButtonCheckedChanged(sender);
        }

        private void choiceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonCheckedChanged(sender);
        }

        #endregion

        private void ButtonCheckedChanged(object sender)
        {
            CheckBox button = sender as CheckBox;

            if (button != null)
            {
                int choiceIndex = (int)button.Tag;
                _question.Choices[choiceIndex].IsCorrect = (bool)button.IsChecked;
            }
        }

        #endregion
    }
}