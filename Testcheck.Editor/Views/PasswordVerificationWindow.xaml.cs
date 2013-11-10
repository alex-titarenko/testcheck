using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for PasswordVerificationWindow.xaml
    /// </summary>
    public partial class PasswordVerificationWindow : Window
    {
        #region Fields

        private string _correctPassword;

        #endregion

        #region Constructors

        public PasswordVerificationWindow(string correctPassword)
        {
            InitializeComponent();

            _correctPassword = correctPassword;
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string actualPassword = passwordBox.Password;

            if (!String.Equals(_correctPassword, actualPassword))
            {
                MessageBox.Show(this, Properties.Resources.locEnteredPasswordIncorrect,
                    Properties.Resources.locErrorMessageCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DialogResult = true;
            }
        }

        #endregion

        #endregion
    }
}
