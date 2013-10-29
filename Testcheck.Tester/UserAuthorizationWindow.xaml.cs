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

namespace TAlex.Testcheck.Tester
{
    /// <summary>
    /// Interaction logic for UserAuthorizationWindow.xaml
    /// </summary>
    public partial class UserAuthorizationWindow : Window
    {
        #region Properties

        public string UserName
        {
            get
            {
                return userNameTextBox.Text;
            }
        }

        public string UserGroup
        {
            get
            {
                return userGroupTextBox.Text;
            }
        }

        #endregion

        #region Constructors

        public UserAuthorizationWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(userNameTextBox.Text.Trim()))
            {
                MessageBox.Show(this, Properties.Resources.UserAuthorizationFailedVerification, "Testcheck",
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
