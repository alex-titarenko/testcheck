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

using TAlex.Testcheck.Tester.TestCore.Questions;

namespace TAlex.Testcheck.Editor.Controls.Editors
{
    /// <summary>
    /// Interaction logic for MatchingEditor.xaml
    /// </summary>
    public partial class MatchingEditor : UserControl
    {
        #region Fields

        private MatchingQuestion _question;

        #endregion

        #region Constructors

        protected MatchingEditor()
        {
            InitializeComponent();
        }

        public MatchingEditor(MatchingQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            matchingModeComboBox.SelectedIndex = (int)_question.MatchingMode;

            leftChoicesGrid.Children.Clear();
            leftChoicesGrid.RowDefinitions.Clear();

            for (int i = 0; i < _question.LeftChoices.Count; i++)
            {
                leftChoicesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                leftChoicesGrid.Children.Add(EditorControlsHelper.CreateMoveUpButton(i, 0, (i != 0), moveUpLeftChoiceButton_Click));
                leftChoicesGrid.Children.Add(EditorControlsHelper.CreateMoveDownButton(i, 1, (i != _question.LeftChoices.Count - 1), moveDownLeftChoiceButton_Click));
                leftChoicesGrid.Children.Add(EditorControlsHelper.CreateChoiceTextBox(i, 2, _question.LeftChoices[i], leftChoiceTextBox_TextChanged));
                leftChoicesGrid.Children.Add(EditorControlsHelper.CreateRemoveChoiceButton(i, 3, removeLeftChoiceButton_Click));
            }

            rightChoicesGrid.Children.Clear();
            rightChoicesGrid.RowDefinitions.Clear();

            for (int i = 0; i < _question.RightChoices.Count; i++)
            {
                rightChoicesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                rightChoicesGrid.Children.Add(EditorControlsHelper.CreateMoveUpButton(i, 0, (i != 0), moveUpRightChoiceButton_Click));
                rightChoicesGrid.Children.Add(EditorControlsHelper.CreateMoveDownButton(i, 1, (i != _question.RightChoices.Count - 1), moveDownRightChoiceButton_Click));
                rightChoicesGrid.Children.Add(EditorControlsHelper.CreateChoiceTextBox(i, 2, _question.RightChoices[i], rightChoiceTextBox_TextChanged));
                rightChoicesGrid.Children.Add(EditorControlsHelper.CreateRemoveChoiceButton(i, 3, removeRightChoiceButton_Click));
            }

            _question.KeyPairs.Sort();
            keyPairsListBox.Items.Clear();

            foreach (KeyPair keyPair in _question.KeyPairs)
            {
                if (!keyPairsListBox.Items.Contains(keyPair))
                {
                    keyPairsListBox.Items.Add(keyPair);
                }
            }
        }

        #region Event Handlers

        private void matchingModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_question != null)
                _question.MatchingMode = (MatchingMode)matchingModeComboBox.SelectedIndex;
        }

        private void moveUpLeftChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 - 1;

            string temp = _question.LeftChoices[key1];
            _question.LeftChoices[key1] = _question.LeftChoices[key2];
            _question.LeftChoices[key2] = temp;

            LoadQuestion();
        }

        private void moveUpRightChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 - 1;

            string temp = _question.RightChoices[key1];
            _question.RightChoices[key1] = _question.RightChoices[key2];
            _question.RightChoices[key2] = temp;

            LoadQuestion();
        }

        private void moveDownLeftChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 + 1;

            string temp = _question.LeftChoices[key1];
            _question.LeftChoices[key1] = _question.LeftChoices[key2];
            _question.LeftChoices[key2] = temp;

            LoadQuestion();
        }

        private void moveDownRightChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 + 1;

            string temp = _question.RightChoices[key1];
            _question.RightChoices[key1] = _question.RightChoices[key2];
            _question.RightChoices[key2] = temp;

            LoadQuestion();
        }

        private void leftChoiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                int answerKey = (int)((FrameworkElement)sender).Tag;
                _question.LeftChoices[answerKey] = textBox.Text;
            }
        }

        private void rightChoiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                int answerKey = (int)((FrameworkElement)sender).Tag;
                _question.RightChoices[answerKey] = textBox.Text;
            }
        }

        private void removeLeftChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), Properties.Resources.RemoveChoiceQuestion, "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int answerKey = (int)((FrameworkElement)sender).Tag;

                _question.LeftChoices.RemoveAt(answerKey);
                LoadQuestion();
            }
        }

        private void removeRightChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), Properties.Resources.RemoveChoiceQuestion, "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int answerKey = (int)((FrameworkElement)sender).Tag;

                _question.RightChoices.RemoveAt(answerKey);
                LoadQuestion();
            }
        }

        private void addLeftChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            _question.LeftChoices.Add(String.Empty);
            LoadQuestion();
        }

        private void addRightChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            _question.RightChoices.Add(String.Empty);
            LoadQuestion();
        }

        private void addKeyPairButton_Click(object sender, RoutedEventArgs e)
        {
            KeyPair newKeyPair = new KeyPair((int)key1NumericUpDown.Value, (int)key2NumericUpDown.Value);

            if (!_question.KeyPairs.Contains(newKeyPair))
            {
                bool containsKey1 = false;
                bool containsKey2 = false;

                foreach (KeyPair item in _question.KeyPairs)
                {
                    if (item.Key1 == newKeyPair.Key1) containsKey1 = true;
                    if (item.Key2 == newKeyPair.Key2) containsKey2 = true;
                }

                switch (_question.MatchingMode)
                {
                    case MatchingMode.OneToMany:
                        if (containsKey1) return;
                        break;

                    case MatchingMode.ManyToOne:
                        if (containsKey2) return;
                        break;

                    case MatchingMode.OneToOne:
                        if (containsKey1 || containsKey2) return;
                        break;
                }

                _question.KeyPairs.Add(newKeyPair);
                _question.KeyPairs.Sort();

                keyPairsListBox.Items.Insert(_question.KeyPairs.IndexOf(newKeyPair), newKeyPair);
            }
        }

        private void keyPairsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (keyPairsListBox.SelectedItems.Count != 0)
            {
                KeyPair keyPair = (KeyPair)keyPairsListBox.SelectedItem;

                key1NumericUpDown.Value = keyPair.Key1;
                key2NumericUpDown.Value = keyPair.Key2;
            }
        }

        private void keyPairsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                RemoveSelectedKeyPairs();
            }
        }

        private void removeKeyPairMenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;

            if (item != null)
                item.IsEnabled = keyPairsListBox.SelectedItems.Count != 0;
        }

        private void removeKeyPairsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedKeyPairs();
        }

        #endregion

        #region Helpers

        private void RemoveSelectedKeyPairs()
        {
            if (keyPairsListBox.SelectedIndex != -1)
            {
                if (MessageBox.Show(Window.GetWindow(this), "Do you really want to delete key pairs?", "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    for (int i = 0; keyPairsListBox.SelectedItems.Count > 0; i++)
                    {
                        _question.KeyPairs.RemoveAt(keyPairsListBox.SelectedIndex);
                        keyPairsListBox.Items.RemoveAt(keyPairsListBox.SelectedIndex);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}