using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TAlex.Testcheck.Core;


namespace TAlex.Testcheck.Tester.Views
{
    /// <summary>
    /// Interaction logic for UserAuthenticationWindow.xaml
    /// </summary>
    public partial class UserAuthenticationWindow : Window
    {
        #region Properties

        public UserInfo UserInfo
        {
            get
            {
                return (UserInfo)DataContext;
            }
        }

        #endregion

        #region Constructors

        public UserAuthenticationWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(UserInfo.Name))
            {
                MessageBox.Show(this, Properties.Resources.UserAuthenticationFailedVerification, "Testcheck",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DialogResult == null || DialogResult == false)
            {
                Environment.Exit(0);
            }
        }

        #endregion

        #endregion
    }
}
