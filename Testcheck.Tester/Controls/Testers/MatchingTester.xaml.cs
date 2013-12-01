using System;
using System.Linq;
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
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for MatchingTester.xaml
    /// </summary>
    [QuestionTester(typeof(MatchingQuestion))]
    public partial class MatchingTester : UserControl
    {
        #region Fields

        private MatchingQuestion _question;

        private LinkLine _currentLinkLine = new LinkLine();

        private Brush _linkLineBrush = Brushes.Silver;

        private TimeSpan _animationDuration = TimeSpan.FromMilliseconds(250.0);

        private DispatcherTimer _stopAnimationTimer = new DispatcherTimer();

        private bool _animationEnded = true;

        #endregion

        #region Constructors

        protected MatchingTester()
        {
            InitializeComponent();

            _stopAnimationTimer.Tick += new EventHandler(stopAnimationTimer_Tick);
            _stopAnimationTimer.Interval = _animationDuration;
        }

        public MatchingTester(MatchingQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            leftChoicesStackPanel.Children.Clear();
            rightChoicesStackPanel.Children.Clear();

            for (int i = 0; i < _question.LeftChoices.Count; i++)
            {
                leftChoicesStackPanel.Children.Add(CreateChoiceItem(_question.LeftChoices[i], i));
            }

            for (int i = 0; i < _question.RightChoices.Count; i++)
            {
                rightChoicesStackPanel.Children.Add(CreateChoiceItem(_question.RightChoices[i], i));
            }

            foreach (KeyPair keyPair in _question.ActualKeyPairs)
            {
                mainGrid.Children.Add(CreateLinkLine(0, 0, 0, 0, keyPair));
            }
        }

        #region Event Handlers

        private void mainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (LinkLine linkLine in mainGrid.Children.Cast<UIElement>().OfType<LinkLine>())
            {
                KeyPair keyPair = (KeyPair)linkLine.Tag;

                TextBlock leftTextBlock = null;
                foreach (TextBlock textBlock in leftChoicesStackPanel.Children)
                {
                    if ((int)textBlock.Tag == keyPair.Key1)
                    {
                        leftTextBlock = textBlock;
                        break;
                    }
                }

                if (leftTextBlock == null)
                    return;

                TextBlock rightTextBlock = null;
                foreach (TextBlock textBlock in rightChoicesStackPanel.Children)
                {
                    if ((int)textBlock.Tag == keyPair.Key2)
                    {
                        rightTextBlock = textBlock;
                        break;
                    }
                }

                if (rightTextBlock == null)
                    return;

                Point p0 = GetAnchorPoint(leftTextBlock, true);
                Point p1 = GetAnchorPoint(rightTextBlock, false);

                linkLine.X1 = p0.X;
                linkLine.Y1 = p0.Y;
                linkLine.X2 = p1.X;
                linkLine.Y2 = p1.Y;
            }
        }

        private void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_animationEnded == false)
                return;
            Cursor = Cursors.Hand;

            Point pos = e.GetPosition((IInputElement)sender);
            IInputElement elem = mainGrid.InputHitTest(pos);

            if (elem is TextBlock)
            {
                TextBlock textBlock = (TextBlock)elem;
                bool isLeftSide = leftChoicesStackPanel.Children.Contains(textBlock);
                Point anchorPoint = GetAnchorPoint(textBlock, isLeftSide);


                if ((isLeftSide && !CanContainsLeftChoice((int)textBlock.Tag)) ||
                    (!isLeftSide && !CanContainsRightChoice((int)textBlock.Tag)))
                {
                    _currentLinkLine = null;
                    return;
                }

                // Create a new link line
                KeyPair keyPair = (isLeftSide) ? new KeyPair((int)textBlock.Tag, -1) : new KeyPair(-1, (int)textBlock.Tag);
                _currentLinkLine = CreateLinkLine(anchorPoint.X, anchorPoint.Y, anchorPoint.X, anchorPoint.Y, keyPair);

                mainGrid.Children.Add(_currentLinkLine);
                mainGrid.CaptureMouse();
            }
            else if (elem is LinkLine)
            {
                _currentLinkLine = (LinkLine)elem;
                _question.ActualKeyPairs.Remove((KeyPair)_currentLinkLine.Tag);

                // Determine the distance from the current point to the ends of the line linking
                double distance1 = Math.Sqrt((_currentLinkLine.X1 - pos.X) * (_currentLinkLine.X1 - pos.X) + (_currentLinkLine.Y1 - pos.Y) * (_currentLinkLine.Y1 - pos.Y));
                double distance2 = Math.Sqrt((_currentLinkLine.X2 - pos.X) * (_currentLinkLine.X2 - pos.X) + (_currentLinkLine.Y2 - pos.Y) * (_currentLinkLine.Y2 - pos.Y));

                KeyPair newKeyPair = (KeyPair)_currentLinkLine.Tag;

                if (distance1 < distance2)
                {
                    FlipLinkLineEndPoints(_currentLinkLine);
                    newKeyPair = new KeyPair(-1, newKeyPair.Key2);
                }
                else
                {
                    newKeyPair = new KeyPair(newKeyPair.Key1, -1);
                }

                _currentLinkLine.Tag = newKeyPair;

                mainGrid.CaptureMouse();
            }
            else
            {
                _currentLinkLine = null;
            }
        }

        private void mainGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IInputElement elem = mainGrid.InputHitTest(e.GetPosition((IInputElement)sender));

            if (elem is LinkLine)
            {
                RemoveLinkLine((LinkLine)elem);
            }
        }

        private void mainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentLinkLine != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition((IInputElement)sender);

                _currentLinkLine.X2 = pos.X;
                _currentLinkLine.Y2 = pos.Y;
            }
        }

        private void mainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainGrid.ReleaseMouseCapture();

            Cursor = null;
            if (_currentLinkLine == null)
                return;

            IInputElement elem = leftChoicesStackPanel.InputHitTest(e.GetPosition(leftChoicesStackPanel));
            if (elem == null)
                elem = rightChoicesStackPanel.InputHitTest(e.GetPosition(rightChoicesStackPanel));

            if (elem is TextBlock)
            {
                TextBlock textBlock = (TextBlock)elem;
                bool isLeftSide = leftChoicesStackPanel.Children.Contains(textBlock);
                KeyPair keyPair = (KeyPair)_currentLinkLine.Tag;

                if ((isLeftSide && keyPair.Key2 == -1) || (!isLeftSide && keyPair.Key1 == -1))
                {
                    RemoveLinkLine(_currentLinkLine);
                }
                else
                {
                    if (isLeftSide)
                        keyPair = new KeyPair((int)textBlock.Tag, keyPair.Key2);
                    else
                        keyPair = new KeyPair(keyPair.Key1, (int)textBlock.Tag);


                    if (!_question.ActualKeyPairs.Contains(keyPair) &&
                        CanContainsLeftChoice(keyPair.Key1) && CanContainsRightChoice(keyPair.Key2))
                    {
                        _question.ActualKeyPairs.Add(keyPair);
                        _currentLinkLine.Tag = keyPair;

                        Point anchorPoint = GetAnchorPoint(textBlock, isLeftSide);                        
                        
                        if (_currentLinkLine.X1 > _currentLinkLine.X2)
                        {
                            FlipLinkLineEndPoints(_currentLinkLine);
                            SetLinkLineStartPoint(_currentLinkLine, anchorPoint.X, anchorPoint.Y);
                        }
                        else
                        {
                            SetLinkLineEndPoint(_currentLinkLine, anchorPoint.X, anchorPoint.Y);
                        }

                        _animationEnded = false;
                        _stopAnimationTimer.Start();
                    }
                    else
                    {
                        RemoveLinkLine(_currentLinkLine);
                    }
                }
            }
            else
            {
                RemoveLinkLine(_currentLinkLine);
            }
        }

        private void stopAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (_currentLinkLine != null)
            {
                double x1 = _currentLinkLine.X1;
                double y1 = _currentLinkLine.Y1;
                double x2 = _currentLinkLine.X2;
                double y2 = _currentLinkLine.Y2;

                _currentLinkLine.BeginAnimation(LinkLine.X1Property, null);
                _currentLinkLine.BeginAnimation(LinkLine.Y1Property, null);
                _currentLinkLine.BeginAnimation(LinkLine.X2Property, null);
                _currentLinkLine.BeginAnimation(LinkLine.Y2Property, null);

                _currentLinkLine.X1 = x1;
                _currentLinkLine.Y1 = y1;
                _currentLinkLine.X2 = x2;
                _currentLinkLine.Y2 = y2;
            }

            _stopAnimationTimer.Stop();
            _animationEnded = true;
        }

        #endregion

        #region Helpers

        private UIElement CreateChoiceItem(string choice, int index)
        {
            return new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Text = choice,
                Tag = index,
                Margin = new Thickness(2),
                Padding = new Thickness(15, 1, 10, 1),
                Background = new SolidColorBrush(Color.FromRgb(235, 235, 235)),
                Cursor = Cursors.Hand
            };
        }

        private LinkLine CreateLinkLine(double x1, double y1, double x2, double y2, KeyPair keyPair)
        {
            LinkLine linkLine = new LinkLine
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = _linkLineBrush,
                StrokeThickness = 2,
                Fill = _linkLineBrush,
                Tag = keyPair
            };

            linkLine.SetValue(Grid.ColumnSpanProperty, 3);
            return linkLine;
        }

        private Point GetAnchorPoint(UIElement elem, bool isLeftSide)
        {
            if (isLeftSide)
                return elem.TranslatePoint(new Point(elem.RenderSize.Width - 5, elem.RenderSize.Height / 2), mainGrid);
            else
                return elem.TranslatePoint(new Point(5, elem.RenderSize.Height / 2), mainGrid);
        }

        private void RemoveLinkLine(FrameworkElement line)
        {
            _question.ActualKeyPairs.Remove((KeyPair)line.Tag);
            mainGrid.Children.Remove(line);
            
            if (line == _currentLinkLine)
                _currentLinkLine = null;
        }

        private void FlipLinkLineEndPoints(LinkLine line)
        {
            double temp = line.X1;
            line.X1 = line.X2;
            line.X2 = temp;

            temp = line.Y1;
            line.Y1 = line.Y2;
            line.Y2 = temp;
        }

        private void SetLinkLineStartPoint(LinkLine line, double x, double y)
        {
            line.BeginAnimation(LinkLine.X1Property, new DoubleAnimation(x, _animationDuration));
            line.BeginAnimation(LinkLine.Y1Property, new DoubleAnimation(y, _animationDuration));
        }

        private void SetLinkLineEndPoint(LinkLine line, double x, double y)
        {
            line.BeginAnimation(LinkLine.X2Property, new DoubleAnimation(x, _animationDuration));
            line.BeginAnimation(LinkLine.Y2Property, new DoubleAnimation(y, _animationDuration));
        }

        private bool CanContainsLeftChoice(int key)
        {
            if (_question.MatchingMode == MatchingMode.ManyToMany ||
                _question.MatchingMode == MatchingMode.ManyToOne)
                return true;

            foreach (var actualKeyPair in _question.ActualKeyPairs)
            {
                if (actualKeyPair.Key1 == key)
                    return false;
            }

            return true;
        }

        private bool CanContainsRightChoice(int key)
        {
            if (_question.MatchingMode == MatchingMode.ManyToMany ||
                _question.MatchingMode == MatchingMode.OneToMany)
                return true;

            foreach (var actualKeyPair in _question.ActualKeyPairs)
            {
                if (actualKeyPair.Key2 == key)
                    return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}