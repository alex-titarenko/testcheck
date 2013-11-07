using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace TAlex.Testcheck.Editor.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        #region Constructors

        protected AboutWindow()
        {
            InitializeComponent();
        }

        public AboutWindow(Window parent)
            : this()
        {
            this.Owner = parent;
        }

        #endregion

        #region Methods

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Close();
        }

        #endregion
    }
}
