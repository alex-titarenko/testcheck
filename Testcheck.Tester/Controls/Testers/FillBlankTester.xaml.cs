using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;

using TAlex.Testcheck.Core.Questions;

namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for FillBlankTester.xaml
    /// </summary>
    public partial class FillBlankTester : UserControl
    {
        #region Fields

        private const string FieldTextBoxNameFormat = "fieldTextBox_a25f0sd456c_{0}";

        private FillBlankQuestion _question;

        #endregion

        #region Constructors

        protected FillBlankTester()
        {
            InitializeComponent();
        }

        public FillBlankTester(Question question)
            : this()
        {
            _question = question as FillBlankQuestion;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            string text = _question.BlankText;

            Regex regex = new Regex(FillBlankQuestion.FieldPattern);

            int fieldIndex = 0;

            var replacements = new List<KeyValuePair<string, string>>();
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                if (_question.ActualAnswers.Count < matches.Count)
                {
                    _question.ActualAnswers.Add(String.Empty);
                }

                string name = String.Format(FieldTextBoxNameFormat, fieldIndex);
                string replacement = String.Format(
                    @"<InlineUIContainer BaselineAlignment='Center'>
                        <TextBox Name='{0}' Height='20' Width='90' Margin='1' Padding='0' Text='{{Binding [" + fieldIndex + @"]}}' />
                    </InlineUIContainer>",
                    name);

                text = regex.Replace(text, name, 1);
                replacements.Add(new KeyValuePair<string, string>(name, replacement));

                fieldIndex++;
            }
            foreach (var replacement in replacements)
            {
                text = text.Replace(replacement.Key, replacement.Value);
            }

            string flowDocumentText = String.Format(
                @"<FlowDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <Paragraph>
                        {0}
                    </Paragraph>
                </FlowDocument>", text);

            FlowDocument doc = (FlowDocument)XamlReader.Parse(flowDocumentText);
            doc.FontFamily = FontFamily;
            doc.FontSize = FontSize;
            doc.PagePadding = new Thickness(3);
            doc.DataContext = _question.ActualAnswers;

            blankTextFlowDocumentScrollViewer.Document = doc;
        }

        #endregion
    }
}
