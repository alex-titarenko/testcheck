using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media;

namespace TAlex.Testcheck.Tester.Controls
{
    public class LinkLine : Shape
    {
        #region Fields

        /// <summary>
        /// Identifies the LinkLine.X1 dependency property.
        /// </summary>
        /// <remarks>
        /// The identifier for the LinkLine.X1 dependency property.
        /// </remarks>
        public static readonly DependencyProperty X1Property;

        /// <summary>
        /// Identifies the LinkLine.X2 dependency property.
        /// </summary>
        /// <remarks>
        /// The identifier for the LinkLine.X2 dependency property.
        /// </remarks>
        public static readonly DependencyProperty X2Property;

        /// <summary>
        /// Identifies the LinkLine.Y1 dependency property.
        /// </summary>
        /// <remarks>
        /// The identifier for the LinkLine.Y1 dependency property.
        /// </remarks>
        public static readonly DependencyProperty Y1Property;

        /// <summary>
        /// Identifies the LinkLine.Y2 dependency property.
        /// </summary>
        /// <remarks>
        /// The identifier for the LinkLine.Y2 dependency property.
        /// </remarks>
        public static readonly DependencyProperty Y2Property;

        /// <summary>
        /// Identifies the LinkLine.RadiusAnchor dependency property.
        /// </summary>
        /// <remarks>
        /// The identifier for the LinkLine.RadiusAnchor dependency property.
        /// </remarks>
        public static readonly DependencyProperty RadiusAnchorProperty;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets a value that represents the Geometry of the LinkLine.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                Point startPoint = new Point(X1, Y1);
                Point endPoint = new Point(X2, Y2);

                GeometryGroup group = new GeometryGroup();
                group.Children.Add(new LineGeometry(startPoint, endPoint));
                group.Children.Add(new EllipseGeometry(startPoint, RadiusAnchor, RadiusAnchor));
                group.Children.Add(new EllipseGeometry(endPoint, RadiusAnchor, RadiusAnchor));

                return group;
            }
        }
        
        /// <summary>
        /// Gets or sets the x-coordinate of the LinkLine start point.
        /// This is a dependency property.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get
            {
                return (double)base.GetValue(X1Property);
            }
            set
            {
                base.SetValue(X1Property, value);
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the LinkLine end point.
        /// This is a dependency property.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get
            {
                return (double)base.GetValue(X2Property);
            }
            set
            {
                base.SetValue(X2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the LinkLine start point.
        /// This is a dependency property.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get
            {
                return (double)base.GetValue(Y1Property);
            }
            set
            {
                base.SetValue(Y1Property, value);
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the LinkLine end point.
        /// This is a dependency property.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get
            {
                return (double)base.GetValue(Y2Property);
            }
            set
            {
                base.SetValue(Y2Property, value);
            }
        }

        public double RadiusAnchor
        {
            get
            {
                return (double)base.GetValue(RadiusAnchorProperty);
            }
            set
            {
                base.SetValue(RadiusAnchorProperty, value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LinkLine class.
        /// </summary>
        static LinkLine()
        {
            X1Property = DependencyProperty.Register("X1", typeof(double), typeof(LinkLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(IsDoubleFinite));
            Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(LinkLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(IsDoubleFinite));
            X2Property = DependencyProperty.Register("X2", typeof(double), typeof(LinkLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(IsDoubleFinite));
            Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(LinkLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(IsDoubleFinite));
            RadiusAnchorProperty = DependencyProperty.Register("RadiusAnchor", typeof(double), typeof(LinkLine), new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(IsDoubleFinite));
        }

        public LinkLine()
        {
        }

        #endregion

        #region Methods

        internal static bool IsDoubleFinite(object o)
        {
            double d = (double)o;
            return (!double.IsInfinity(d) && !double.IsNaN(d));
        }

        #endregion
    }
}
