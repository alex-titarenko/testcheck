using System;
using System.Collections.Generic;
using System.Text;

using System.Windows;
using System.Windows.Media.Animation;

namespace TAlex.Testcheck.Tester.Controls
{
    public class GridLengthAnimation : AnimationTimeline
    {
        #region Fields

        public static readonly DependencyProperty ByProperty;

        public static readonly DependencyProperty FromProperty;

        public static readonly DependencyProperty ToProperty;

        #endregion

        #region Properties

        public sealed override Type TargetPropertyType
        {
            get
            {
                base.ReadPreamble();
                return typeof(GridLength);
            }
        }

        public GridLength? By
        {
            get
            {
                return (GridLength?)base.GetValue(ByProperty);
            }
            set
            {
                base.SetValue(ByProperty, value);
            }
        }
 
        public GridLength? From
        {
            get
            {
                return (GridLength?)base.GetValue(FromProperty);
            }
            set
            {
                base.SetValue(FromProperty, value);
            }
        }

        public GridLength? To
        {
            get
            {
                return (GridLength?)base.GetValue(ToProperty);
            }
            set
            {
                base.SetValue(ToProperty, value);
            }
        }

        #endregion

        #region Constructors

        static GridLengthAnimation()
        {
            Type propertyType = typeof(GridLength?);
            Type ownerType = typeof(GridLengthAnimation);

            FromProperty = DependencyProperty.Register("From", propertyType, ownerType, new PropertyMetadata(null));
            ToProperty = DependencyProperty.Register("To", propertyType, ownerType, new PropertyMetadata(null));
            ByProperty = DependencyProperty.Register("By", propertyType, ownerType, new PropertyMetadata(null));
        }

        public GridLengthAnimation()
        {
        }

        #endregion

        #region Methods

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public sealed override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (defaultOriginValue == null)
            {
                throw new ArgumentNullException("defaultOriginValue");
            }
            if (defaultDestinationValue == null)
            {
                throw new ArgumentNullException("defaultDestinationValue");
            }
            return this.GetCurrentValue((GridLength)defaultOriginValue, (GridLength)defaultDestinationValue, animationClock);
        }

        protected GridLength GetCurrentValue(GridLength defaultOriginValue, GridLength defaultDestinationValue, AnimationClock animationClock)
        {
            base.ReadPreamble();
            if (animationClock == null)
            {
                throw new ArgumentNullException("animationClock");
            }
            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }

            DoubleAnimation doubleAnim = new DoubleAnimation(To.Value.Value, Duration);
            double currentValue = doubleAnim.GetCurrentValue(defaultOriginValue.Value, defaultDestinationValue.Value, animationClock);
            return new GridLength(currentValue, GridUnitType.Star);
        }

        #endregion
    }
}
