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

        private MultipleResponseQuestion _question;

        #endregion

        #region Constructors

        protected MultipleResponseTester()
        {
            InitializeComponent();
        }

        public MultipleResponseTester(MultipleResponseQuestion question, Random rand)
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

            int[] indexes = TAlex.Testcheck.Core.Helpers.Shuffles.GetRandomSequence(_question.Choices.Count, _question.ShuffleMode, rand);

            for (int i = 0; i < _question.Choices.Count; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Margin = new Thickness(0, 1, 0, 1);

                TextBlock textBlock = new TextBlock();
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Text = _question.Choices[indexes[i]].Description;

                checkBox.Content = textBlock;
                checkBox.Tag = indexes[i];

                choicesStackPanel.Children.Add(checkBox);
            }
        }

        public decimal Check()
        {
            List<int> choiceIndexes = new List<int>();

            foreach (CheckBox checkBox in choicesStackPanel.Children)
            {
                if (checkBox.IsChecked == true)
                    choiceIndexes.Add((int)checkBox.Tag);
            }

            return _question.Check(choiceIndexes.ToArray());
        }

        #endregion
    }
}
