using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TAlex.Testcheck.Core;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Editor.Controls.Editors;

using TAlex.WPF.Controls;
using TAlex.Testcheck.Editor.Locators;
using TAlex.Testcheck.Editor.Services.Licensing;

namespace TAlex.Testcheck.Editor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private const string DefaultFilePath = "Untitled";
        private const string OpenSaveDialogFilter = "Xml file test (*.xml)|*.xml|Binary file test (*.tst)|*.tst";

        private Test _initialTest;
        private Test _currentTest;

        private int _currentQuestionIndex;

        private string _currentFilePath;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            string productTitle = TAlex.Common.Environment.ApplicationInfo.Current.Title;
            Title = productTitle;

            aboutMenuItem.Header = "_About " + productTitle;
        }

        #endregion

        #region Methods

        #region Event Handlers

        #region Command Bindings

        private void CommandBindingNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SaveAsBeforeAction() == MessageBoxResult.Cancel)
                return;

            _initialTest = new Test();
            _currentTest = new Test();
            LoadTestToUI(_currentTest);

            _currentFilePath = DefaultFilePath;
            SetTitle(_currentFilePath);
        }

        private void CommandBindingOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SaveAsBeforeAction() == MessageBoxResult.Cancel)
                return;

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = OpenSaveDialogFilter;
            ofd.FilterIndex = 2;

            if (ofd.ShowDialog(this) == true)
            {
                LoadTest(ofd.FileName);
            }
        }

        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !Test.Equals(_initialTest, _currentTest);
        }

        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_currentFilePath == null)
            {
                return;
            }

            if (_currentFilePath == DefaultFilePath)
            {
                ApplicationCommands.SaveAs.Execute(null, null);
            }
            else if (!String.IsNullOrEmpty(_currentFilePath))
            {
                _initialTest = (Test)_currentTest.Clone();
                _currentTest.Save(_currentFilePath);
            }
        }

        private void CommandBindingSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(_currentFilePath);
            sfd.Filter = OpenSaveDialogFilter;
            sfd.FilterIndex = 2;

            if (sfd.ShowDialog(this) == true)
            {
                _currentFilePath = sfd.FileName;
                SetTitle(System.IO.Path.GetFileName(_currentFilePath));

                _initialTest = (Test)_currentTest.Clone();
                _currentTest.Save(_currentFilePath);
            }
        }

        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (SaveAsBeforeAction() == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 2)
            {
                LoadTest(args[1]);
            }
            else
            {
                ApplicationCommands.New.Execute(null, null);
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void homepageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Properties.Resources.HomepageUrl);
        }

        private void registrationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new RegistrationWindow { Owner = this }.ShowDialog();
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow(this);
            window.ShowDialog();
        }


        private void timelimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentTest != null)
            {
                try
                {
                    TimeSpan newTimelimit = TimeSpan.Parse(timelimitTextBox.Text);
                    _currentTest.Timelimit = newTimelimit;

                    timelimitTextBox.Foreground = Brushes.Black;
                }
                catch (Exception exc)
                {
                    if (exc is FormatException || exc is ArgumentOutOfRangeException || exc is OverflowException)
                    {
                        timelimitTextBox.Foreground = Brushes.Red;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void noTimelimitHyperlink_Click(object sender, RoutedEventArgs e)
        {
            timelimitTextBox.Text = TimeSpan.Zero.ToString();
        }

        private void questionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentQuestionIndex = questionsListBox.SelectedIndex;

            if (_currentQuestionIndex != -1)
            {
                questionInformationGroupBox.IsEnabled = true;
                LoadQuestionToUI(_currentTest.Questions[_currentQuestionIndex]);
            }
            else
            {
                LoadQuestionToUI(null);
                questionInformationGroupBox.IsEnabled = false;
            }
        }

        private void addQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            AddQuestionWindow window = new AddQuestionWindow();
            window.Owner = this;

            if (window.ShowDialog() == true)
            {
                _currentTest.Questions.Add(window.NewQuestion);
                LoadTestToUI(_currentTest);

                questionsListBox.SelectedIndex = _currentTest.QuestionCount - 1;
            }
        }

        private void removeQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (questionsListBox.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show(this, Properties.Resources.RemoveQuestionsQuestion, "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                List<int> selectedIndexes = new List<int>();

                foreach (Object item in questionsListBox.SelectedItems)
                {
                    selectedIndexes.Add(questionsListBox.Items.IndexOf(item));
                }

                selectedIndexes.Sort();
                selectedIndexes.Reverse();

                foreach (int index in selectedIndexes)
                {
                    _currentTest.Questions.RemoveAt(index);
                }

                LoadTestToUI(_currentTest);
            }
        }

        private void questionTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentQuestionIndex != -1)
            {
                string title = questionTitleTextBox.Text;

                _currentTest.Questions[_currentQuestionIndex].Title = title;

                questionsListBox.SelectionChanged -= new SelectionChangedEventHandler(questionsListBox_SelectionChanged);

                if (String.IsNullOrEmpty(title))
                    questionsListBox.Items[_currentQuestionIndex] = questionsListBox.SelectedItem = String.Format("Question {0}", _currentQuestionIndex + 1);
                else
                    questionsListBox.Items[_currentQuestionIndex] = questionsListBox.SelectedItem = title;

                questionsListBox.SelectionChanged += new SelectionChangedEventHandler(questionsListBox_SelectionChanged);
                questionsListBox.SelectedIndex = _currentQuestionIndex;
            }
        }

        private void questionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentQuestionIndex != -1)
                _currentTest.Questions[_currentQuestionIndex].Description = questionTextBox.Text;
        }

        private void shuffleModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentTest != null && _currentQuestionIndex != -1)
            {
                ((IShuffles)_currentTest.Questions[_currentQuestionIndex]).ShuffleMode = (ShuffleMode)shuffleModeComboBox.SelectedIndex;
            }
        }

        private void passwordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _currentTest.Password = passwordTextBox.Password;
        }

        #endregion

        private bool LoadTest(string path)
        {
            try
            {
                Test test = Test.Load(path);
                if (!String.IsNullOrWhiteSpace(test.Password))
                {
                    PasswordVerificationWindow passVerifWindows = new PasswordVerificationWindow { Owner = this };
                    if (passVerifWindows.ShowDialog() != true)
                    {
                        return false;
                    }
                }

                _initialTest = test;
                _currentTest = (Test)_initialTest.Clone();

                LoadTestToUI(_currentTest);

                _currentFilePath = path;
                SetTitle(System.IO.Path.GetFileName(_currentFilePath));

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Can not load the test file. Test file is corrupted.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void LoadTestToUI(Test test)
        {
            titleTestTextBox.SetBinding(TextBox.TextProperty, new Binding("Title") { Source = test, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            descriptionTestTextBox.SetBinding(TextBox.TextProperty, new Binding("Description") { Source = test, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            authorTestTextBox.SetBinding(TextBox.TextProperty, new Binding("Author") { Source = test, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            copyrightTestTextBox.SetBinding(TextBox.TextProperty, new Binding("Copyright") { Source = test, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            timelimitTextBox.Text = test.Timelimit.ToString();

            shuffleQuestionsCheckBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("ShuffleQuestions") { Source = test, Mode = BindingMode.TwoWay });
            gradingScaleNumericUpDown.SetBinding(NumericUpDown.ValueProperty, new Binding("GradingScale") { Source = test, Mode = BindingMode.TwoWay });

            questionsListBox.Items.Clear();

            for (int i = 0; i < test.QuestionCount; i++)
            {
                if (!String.IsNullOrEmpty(test.Questions[i].Title))
                    questionsListBox.Items.Add(test.Questions[i].Title);
                else
                    questionsListBox.Items.Add(String.Format("Question {0}", i + 1));
            }

            LoadQuestionToUI(null);
            questionInformationGroupBox.IsEnabled = false;
        }

        private void LoadQuestionToUI(Question question)
        {
            questionEditor.Children.Clear();


            if (question == null)
            {
                questionTypeLabel.Content = null;
                questionTitleTextBox.Text = String.Empty;

                questionPointsNumericUpDown.SetBinding(NumericUpDown.ValueProperty, new Binding());

                questionTextBox.Text = String.Empty;
                questionAddInfoGrid.RowDefinitions[0].Height = new GridLength(0);
                return;
            }

            questionTypeLabel.Content = question.TypeName;
            questionTitleTextBox.Text = question.Title;
            questionTextBox.Text = question.Description;

            questionPointsNumericUpDown.SetBinding(NumericUpDown.ValueProperty,
                new Binding("Points") { Source = question, Mode = BindingMode.TwoWay });


            if (question is IShuffles)
            {
                questionAddInfoGrid.RowDefinitions[0].Height = new GridLength(35);
                shuffleModeComboBox.SelectedIndex = (int)((IShuffles)question).ShuffleMode;
            }
            else
            {
                questionAddInfoGrid.RowDefinitions[0].Height = new GridLength(0);
            }

            if (question is TrueFalseQuestion)
                questionEditor.Children.Add(new TrueFalseEditor((TrueFalseQuestion)question));
            else if (question is MultipleChoiceQuestion)
                questionEditor.Children.Add(new MultipleChoiceEditor((MultipleChoiceQuestion)question));
            else if (question is MultipleResponseQuestion)
                questionEditor.Children.Add(new MultipleResponseEditor((MultipleResponseQuestion)question));
            else if (question is EssayQuestion)
                questionEditor.Children.Add(new EssayEditor((EssayQuestion)question));
            else if (question is FillBlankQuestion)
                questionEditor.Children.Add(new FillBlankEditor((FillBlankQuestion)question));
            else if (question is MatchingQuestion)
                questionEditor.Children.Add(new MatchingEditor((MatchingQuestion)question));
            else if (question is RankingQuestion)
                questionEditor.Children.Add(new RankingEditor((RankingQuestion)question));
        }

        #region Helpers

        private void SetTitle(string filename)
        {
            ViewModelLocator locator = App.Current.Resources["viewModelLocator"] as ViewModelLocator;
            AppLicense license = locator.Get<AppLicense>();

            string title = String.Format("{0} - {1}", TAlex.Common.Environment.ApplicationInfo.Current.Title, filename);
            if (license.IsTrial)
            {
                title = String.Format("{0} (days left: {1})", title, license.TrialDaysLeft);
            }

            Title = title;
        }

        private MessageBoxResult SaveAsBeforeAction()
        {
            if (_currentTest != null)
            {
                if (!Test.Equals(_currentTest, _initialTest))
                {
                    MessageBoxResult result = MessageBox.Show(this,
                        String.Format("Do you want to save changes to {0} before the action?", System.IO.Path.GetFileName(_currentFilePath)),
                        "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        ApplicationCommands.SaveAs.Execute(null, null);
                    }

                    return result;
                }
            }

            return MessageBoxResult.None;
        }

        #endregion

        #endregion
    }
}
