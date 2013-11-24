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
    /// Interaction logic for RankingTester.xaml
    /// </summary>
    public partial class RankingTester : UserControl
    {
        #region Fields

        private RankingQuestion _question;

        #endregion

        #region Constructors

        protected RankingTester()
        {
            InitializeComponent();
        }

        public RankingTester(Question question)
            : this()
        {
            _question = question as RankingQuestion;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            choicesStackPanel.Children.Clear();

            Style buttonsStyle = (Style)Resources["SimpleButton"];

            foreach (var choice in _question.Choices)
            {
                int index = _question.Choices.IndexOf(choice);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(22) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(22) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                
                Button moveUpButton = new Button();
                moveUpButton.Content = new Image() { Source = new BitmapImage(new Uri(@"/Resources/Images/up.png", UriKind.Relative)) };
                moveUpButton.ToolTip = "Move Up";
                moveUpButton.Margin = new Thickness(1, 0.5, 1, 0.5);
                moveUpButton.Style = buttonsStyle;
                moveUpButton.Height = 18;
                moveUpButton.Tag = index;
                moveUpButton.Focusable = false;
                moveUpButton.IsEnabled = (index != 0);
                moveUpButton.SetValue(Grid.ColumnProperty, 0);
                moveUpButton.Click += new RoutedEventHandler(moveUpButton_Click);

                Button moveDownButton = new Button();
                moveDownButton.Content = new Image() { Source = new BitmapImage(new Uri(@"/Resources/Images/down.png", UriKind.Relative)) };
                moveDownButton.ToolTip = "Move Down";
                moveDownButton.Margin = new Thickness(1, 0.5, 1, 0.5);
                moveDownButton.Style = buttonsStyle;
                moveDownButton.Height = 18;
                moveDownButton.Tag = index;
                moveDownButton.Focusable = false;
                moveDownButton.IsEnabled = (index != _question.Choices.Count - 1);
                moveDownButton.SetValue(Grid.ColumnProperty, 1);
                moveDownButton.Click += new RoutedEventHandler(moveDownButton_Click);

                TextBlock textBlock = new TextBlock();
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.SetValue(Grid.ColumnProperty, 3);
                textBlock.Margin = new Thickness(2);
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Text = choice.Choice;

                grid.Children.Add(moveUpButton);
                grid.Children.Add(moveDownButton);
                grid.Children.Add(textBlock);

                choicesStackPanel.Children.Add(grid);
            }
        }

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 - 1;

            SwapChoices(key1, key2);
            LoadQuestion();
        }

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 + 1;

            SwapChoices(key1, key2);
            LoadQuestion();
        }

        private void SwapChoices(int idx1, int idx2)
        {
            var temp = _question.Choices[idx1];
            _question.Choices[idx1] = _question.Choices[idx2];
            _question.Choices[idx2] = temp;
        }

        #endregion
    }
}
