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

namespace TAlex.Testcheck.Tester.Controls
{
    /// <summary>
    /// Interaction logic for ProgressBarTest.xaml
    /// </summary>
    public partial class TestProgressBar : UserControl
    {
        #region Fields

        public static readonly DependencyProperty TotalPointsProperty;
        public static readonly DependencyProperty ScoredPointsProperty;
        public static readonly DependencyProperty PossiblePointsProperty;
        public static readonly DependencyProperty AnimationDurationProperty;

        #endregion

        #region Properties

        public decimal TotalPoints
        {
            get
            {
                return (decimal)GetValue(TotalPointsProperty);
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("TotalPoints");

                SetValue(TotalPointsProperty, value);
            }
        }

        public decimal PossiblePoints
        {
            get
            {
                return (decimal)GetValue(PossiblePointsProperty);
            }

            set
            {
                SetValue(PossiblePointsProperty, value);
            }
        }

        public decimal ScoredPoints
        {
            get
            {
                return (decimal)GetValue(ScoredPointsProperty);
            }

            set
            {
                if (value < 0 || value > PossiblePoints)
                    throw new ArgumentOutOfRangeException("ScoredPoints");

                SetValue(ScoredPointsProperty, value);
            }
        }

        public TimeSpan AnimationDuration
        {
            get
            {
                return (TimeSpan)GetValue(AnimationDurationProperty);
            }

            set
            {
                SetValue(AnimationDurationProperty, value);
            }
        }

        #endregion

        #region Constructors

        static TestProgressBar()
        {
            TotalPointsProperty = DependencyProperty.Register("TotalPoints", typeof(decimal), typeof(TestProgressBar), new PropertyMetadata(BasePropertyChanged));
            PossiblePointsProperty = DependencyProperty.Register("PossiblePoints", typeof(decimal), typeof(TestProgressBar), new PropertyMetadata(BasePropertyChanged));
            ScoredPointsProperty = DependencyProperty.Register("ScoredPoints", typeof(decimal), typeof(TestProgressBar), new PropertyMetadata(BasePropertyChanged));
            AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(TestProgressBar), new PropertyMetadata(TimeSpan.FromMilliseconds(500)));
        }

        public TestProgressBar()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private static void BasePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TestProgressBar)d).UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            if (TotalPoints != 0)
            {
                progressLabel.Content = (PossiblePoints / TotalPoints).ToString("P");

                GridLengthAnimation anim = new GridLengthAnimation();
                anim.Duration = AnimationDuration;

                anim.To = new GridLength((double)(1 - PossiblePoints / TotalPoints), GridUnitType.Star);
                progressGrid.RowDefinitions[0].BeginAnimation(RowDefinition.HeightProperty, anim);

                anim.To = new GridLength((double)((PossiblePoints - ScoredPoints) / TotalPoints), GridUnitType.Star);
                progressGrid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, anim);

                anim.To = new GridLength((double)(ScoredPoints / TotalPoints), GridUnitType.Star);
                progressGrid.RowDefinitions[2].BeginAnimation(RowDefinition.HeightProperty, anim);
            }
            else
            {
                progressLabel.Content = (0).ToString("P");
            }
        }

        #endregion
    }
}
