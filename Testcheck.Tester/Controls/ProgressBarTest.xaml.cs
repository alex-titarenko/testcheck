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
    public partial class ProgressBarTest : UserControl
    {
        #region Fields

        private decimal _maxValue;
        private decimal _totalCurrentValue;
        private decimal _correctCurrentValue;

        private TimeSpan _animationDuration;

        #endregion

        #region Properties

        public decimal Maximum
        {
            get
            {
                return _maxValue;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("MaxValue");

                _maxValue = value;
                Update();
            }
        }

        public decimal TotalCurrentValue
        {
            get
            {
                return _totalCurrentValue;
            }

            set
            {
                if (value < 0)
                    _totalCurrentValue = 0;
                else if (value > _maxValue)
                    _totalCurrentValue = _maxValue;
                else
                    _totalCurrentValue = value;

                Update();
            }
        }

        public decimal CorrectCurrentValue
        {
            get
            {
                return _correctCurrentValue;
            }

            set
            {
                if (value < 0 || value > _totalCurrentValue)
                    throw new ArgumentOutOfRangeException("Score");

                _correctCurrentValue = value;
                Update();
            }
        }

        public TimeSpan AnimationDuration
        {
            get
            {
                return _animationDuration;
            }

            set
            {
                _animationDuration = value;
            }
        }

        #endregion

        #region Constructors

        public ProgressBarTest()
        {
            InitializeComponent();

            _animationDuration = TimeSpan.FromMilliseconds(500);
        }

        #endregion

        #region Methods

        private void Update()
        {
            if (_maxValue != 0)
            {
                progressLabel.Content = (_totalCurrentValue / _maxValue).ToString("P");

                GridLengthAnimation anim = new GridLengthAnimation();
                anim.Duration = _animationDuration;

                anim.To = new GridLength((double)(1 - _totalCurrentValue / _maxValue), GridUnitType.Star);
                progressGrid.RowDefinitions[0].BeginAnimation(RowDefinition.HeightProperty, anim);

                anim.To = new GridLength((double)((_totalCurrentValue - _correctCurrentValue) / _maxValue), GridUnitType.Star);
                progressGrid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, anim);

                anim.To = new GridLength((double)(_correctCurrentValue / _maxValue), GridUnitType.Star);
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
