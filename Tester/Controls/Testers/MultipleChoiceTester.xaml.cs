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

namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for MultipleChoiceTester.xaml
    /// </summary>
    public partial class MultipleChoiceTester : UserControl, ICheckable
    {
        #region Fields

        private MultipleChoiceQuestion _question;

        #endregion

        #region Constructors

        protected MultipleChoiceTester()
        {
            InitializeComponent();
        }

        public MultipleChoiceTester(MultipleChoiceQuestion question, Random rand)
            : this()
        {
            _question = question;
            LoadQuestion(rand);
        }

        #endregion

        #region Methods

        private void LoadQuestion(Random rand)
        {
            choicesStackPanel.Children.Clear();

            int[] indexes = Helpers.Shuffles.GetRandomSequence(_question.Choices.Count, _question.ShuffleMode, rand);

            for (int i = 0; i < _question.Choices.Count; i++)
            {
                RadioButton button = new RadioButton();
                button.Margin = new Thickness(0, 1, 0, 1);

                TextBlock textBlock = new TextBlock();
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Text = _question.Choices[indexes[i]];

                button.Content = textBlock;
                button.Tag = indexes[i];

                choicesStackPanel.Children.Add(button);
            }
        }

        public decimal Check()
        {
            int key = -1;

            foreach (RadioButton button in choicesStackPanel.Children)
            {
                if (button.IsChecked == true)
                {
                    key = (int)button.Tag;
                    break;
                }
            }

            return _question.Check(key);
        }

        #endregion
    }
}
