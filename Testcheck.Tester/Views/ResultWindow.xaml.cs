using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using TAlex.Testcheck.Tester.Reporting;


namespace TAlex.Testcheck.Tester.Views
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        #region Constructors

        public ResultWindow(TestReport testReport)
        {
            InitializeComponent();
            DataContext = testReport;
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        #endregion
    }
}
