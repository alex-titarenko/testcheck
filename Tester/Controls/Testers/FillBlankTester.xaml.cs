using System;
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

using TAlex.Testcheck.Tester.TestCore.Questions;

namespace TAlex.Testcheck.Tester.Controls.Testers
{
    /// <summary>
    /// Interaction logic for FillBlankTester.xaml
    /// </summary>
    public partial class FillBlankTester : UserControl, ICheckable
    {
        #region Fields

        private const string FieldTextBoxNameFormat = "fieldTextBox_a25f0sd456c_{0}";

        private FillBlankQuestion _question;

        private int _fieldCount = 0;

        #endregion

        #region Constructors

        protected FillBlankTester()
        {
            InitializeComponent();
        }

        public FillBlankTester(FillBlankQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            string text = _question.BlankText;

            Regex regex = new Regex(FillBlankQuestion.FieldPattern);

            _fieldCount = 0;
            while (regex.IsMatch(text))
            {
                string name = String.Format(FieldTextBoxNameFormat, _fieldCount);
                string replacement = String.Format("<InlineUIContainer BaselineAlignment='Center'><TextBox Name='{0}' Height='20' Width='90' Margin='1' Padding='0'></TextBox></InlineUIContainer>", name);
                text = regex.Replace(text, replacement, 1);

                _fieldCount++;
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
            
            blankTextFlowDocumentScrollViewer.Document = doc;
        }

        public decimal Check()
        {
            string[] fields = new string[_fieldCount];

            FlowDocument doc = blankTextFlowDocumentScrollViewer.Document;
            for (int i = 0; i < _fieldCount; i++)
            {
                string textBoxName = String.Format(FieldTextBoxNameFormat, i);

                TextBox fieldTextBox = doc.FindName(textBoxName) as TextBox;
                if (fieldTextBox != null)
                {
                    fields[i] = fieldTextBox.Text;
                }
            }

            return _question.Check(fields);
        }

        #endregion
    }
}
