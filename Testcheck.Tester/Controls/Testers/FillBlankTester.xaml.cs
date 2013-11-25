using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Infrastructure;


namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for FillBlankTester.xaml
    /// </summary>
    [QuestionTester(typeof(FillBlankQuestion))]
    public partial class FillBlankTester : UserControl
    {
        #region Fields

        private const string FieldTextBoxNameFormat = "fieldTextBox_a25f0sd456c_{0}";

        #endregion

        #region Constructors

        protected FillBlankTester()
        {
            InitializeComponent();
        }

        public FillBlankTester(FillBlankQuestion question)
            : this()
        {
            LoadQuestion(question);
        }

        #endregion

        #region Methods

        private void LoadQuestion(FillBlankQuestion question)
        {
            string text = question.BlankText;

            Regex regex = new Regex(FillBlankQuestion.FieldPattern);

            int fieldIndex = 0;

            var replacements = new List<KeyValuePair<string, string>>();
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                if (question.ActualAnswers.Count < matches.Count)
                {
                    question.ActualAnswers.Add(String.Empty);
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
            doc.DataContext = question.ActualAnswers;

            blankTextFlowDocumentScrollViewer.Document = doc;
        }

        #endregion
    }
}
