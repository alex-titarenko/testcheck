using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for RankingTester.xaml
    /// </summary>
    [QuestionTester(typeof(RankingQuestion))]
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

        public RankingTester(RankingQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            choicesStackPanel.Children.Clear();

            Style buttonStyle = (Style)Resources["SimpleButton"];

            foreach (var choice in _question.Choices)
            {
                int index = _question.Choices.IndexOf(choice);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(22) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(22) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });


                Button moveUpButton = CreateMoveChoiceButton(@"/Resources/Images/up.png", "Move Up", index, buttonStyle, index != 0);
                moveUpButton.SetValue(Grid.ColumnProperty, 0);
                moveUpButton.Click += new RoutedEventHandler(moveUpButton_Click);

                Button moveDownButton = CreateMoveChoiceButton(@"/Resources/Images/down.png", "Move Down", index, buttonStyle, index != _question.Choices.Count - 1);
                moveDownButton.SetValue(Grid.ColumnProperty, 1);
                moveDownButton.Click += new RoutedEventHandler(moveDownButton_Click);

                TextBlock textBlock = new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2),
                    TextWrapping = TextWrapping.Wrap,
                    Text = choice.Choice
                };
                textBlock.SetValue(Grid.ColumnProperty, 3);


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

        private Button CreateMoveChoiceButton(string imageUri, string toolTip, int index, Style buttonStyle, bool isEnabled)
        {
            Button moveUpButton = new Button
            {
                Content = new Image() { Source = new BitmapImage(new Uri(imageUri, UriKind.Relative)) },
                ToolTip = toolTip,
                Margin = new Thickness(1, 0.5, 1, 0.5),
                Style = buttonStyle,
                Height = 18,
                Tag = index,
                Focusable = false,
                IsEnabled = isEnabled
            };

            return moveUpButton;
        }

        #endregion
    }
}
