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
using System.Windows.Shapes;
using System.IO;

using TAlex.Testcheck.Tester.TestCore;

namespace TAlex.Testcheck.Tester
{
    /// <summary>
    /// Interaction logic for ChoiceTestWindow.xaml
    /// </summary>
    public partial class ChoiceTestWindow : Window
    {
        #region Fields

        private static string TestDirPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            System.IO.Path.Combine("Testcheck", "Tests"));

        #endregion

        #region Properties

        public Test SelectedTest
        {
            get
            {
                return (Test)testsListView.SelectedItem;
            }
        }

        #endregion

        #region Constructors

        public ChoiceTestWindow()
        {
            InitializeComponent();
            LoadTests();
        }

        #endregion

        #region Methods

        private void LoadTests()
        {
            if (Directory.Exists(TestDirPath))
            {
                string[] filePaths = Directory.GetFiles(TestDirPath);

                for (int i = 0; i < filePaths.Length; i++)
                {
                    string extension = System.IO.Path.GetExtension(filePaths[i]);

                    if (extension == Test.BinaryTestFileExtension || extension == Test.XmlTestFileExtension)
                    {
                        try
                        {
                            Test test = Test.Load(filePaths[i]);
                            testsListView.Items.Add(test);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        #region Event Handlers

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (testsListView.SelectedItems.Count == 1)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show(this, "Please select the test!", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
