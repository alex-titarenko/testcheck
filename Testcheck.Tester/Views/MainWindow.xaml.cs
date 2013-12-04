using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TAlex.Testcheck.Core;
using TAlex.Testcheck.Tester.ViewModels;
using TAlex.Testcheck.Tester.Infrastructure.UI;


namespace TAlex.Testcheck.Tester.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            Test test = null;

            if (args.Length == 2)
            {
                try
                {
                    test = Test.Load(args[1]);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, String.Format(Properties.Resources.CanNotLoadFile, args[1]),
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    Application.Current.Shutdown();
                }
            }
            else
            {
                ChoiceTestWindow window = new ChoiceTestWindow { Owner = this };
                if (window.ShowDialog() == true && window.DialogResult == true)
                {
                    test = window.SelectedTest;
                }
            }

            // User authorization
            UserAuthenticationWindow authWindow = new UserAuthenticationWindow { Owner = this };

            if (authWindow.ShowDialog() == true)
            {
                LoadTest(test, authWindow.UserInfo);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Do you really want to stop testing? All results will be lost.", "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void questionWebBrowser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                case Key.BrowserRefresh:
                case Key.Apps:
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        private void LoadTest(Test test, UserInfo userInfo)
        {
            Title = String.Format("Testcheck Tester - {0}", test.Title);
            
            test.Shuffle();
            DataContext = new TesterViewModel(test, userInfo, new TestResultDialogService());
        }

        #endregion
    }
}
