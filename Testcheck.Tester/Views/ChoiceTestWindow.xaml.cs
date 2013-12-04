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
using TAlex.Testcheck.Core;


namespace TAlex.Testcheck.Tester.Views
{
    /// <summary>
    /// Interaction logic for ChoiceTestWindow.xaml
    /// </summary>
    public partial class ChoiceTestWindow : Window
    {
        #region Fields

        private static readonly string CommonDocumentsTestDir = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Testcheck", "Tests");

        private static readonly string MyDocumentsTestDir = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Testcheck", "Tests");

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
            LoadTests(new[] { CommonDocumentsTestDir, MyDocumentsTestDir });
        }

        #endregion

        #region Methods

        private void LoadTests(IEnumerable<string> paths)
        {
            testsListView.Items.Clear();
            foreach (string path in paths)
            {
                foreach (Test test in LoadTests(path))
                {
                    testsListView.Items.Add(test);
                }
            }
        }

        private IEnumerable<Test> LoadTests(string targetDirectory)
        {
            if (Directory.Exists(targetDirectory))
            {
                foreach (string filePath in Directory.GetFiles(targetDirectory))
                {
                    string extension = System.IO.Path.GetExtension(filePath);

                    if (extension == Test.BinaryTestFileExtension || extension == Test.XmlTestFileExtension)
                    {
                        Test test = null;
                        try
                        {
                            test = Test.Load(filePath);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        yield return test;
                    }
                }
            }
        }

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            testsListView.SelectedIndex = 0;
        }

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
