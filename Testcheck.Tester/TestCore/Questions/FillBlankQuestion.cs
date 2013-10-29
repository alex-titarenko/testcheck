using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace TAlex.Testcheck.Tester.TestCore.Questions
{
    /// <summary>
    /// Represents the fill in the blank of the type of question.
    /// </summary>
    /// <remarks>
    /// Participants are expected to type in the text of an answer.
    /// The answer is assessed by regular expressions.
    /// </remarks>
    [Serializable]
    public class FillBlankQuestion : Question
    {
        #region Fields

        public const string FieldPattern = @"\[(?<field>[\S]*)\]";

        private const string BlankTextElemName = "BlankText";

        private string _blankText = String.Empty;

        #endregion

        #region Properties

        public string BlankText
        {
            get
            {
                return _blankText;
            }

            set
            {
                _blankText = value;
            }
        }

        public override string TypeName
        {
            get
            {
                return "Fill in the Blank";
            }
        }

        #endregion

        #region Constructors

        public FillBlankQuestion()
        {
        }

        protected FillBlankQuestion(FillBlankQuestion question)
            : base(question)
        {
            if (question._blankText != null)
                _blankText = (string)question._blankText.Clone();
        }

        #endregion

        #region Methods

        public override decimal Check(object data)
        {
            return Check(data as string[]);
        }

        public decimal Check(string[] fields)
        {
            MatchCollection matches = Regex.Matches(_blankText, FieldPattern);
            int count = fields.Length;

            if (matches.Count != count)
                throw new ArgumentOutOfRangeException();

            decimal pointValue = 0;
            decimal pointValuePerAnswer = 1M / count;

            for (int i = 0; i < count; i++)
            {
                if (matches[i].Groups["field"].Value.ToUpper() == fields[i].ToUpper())
                    pointValue += pointValuePerAnswer;
            }

            return pointValue * Points;
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            BlankText = element[BlankTextElemName].InnerXml;
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteStartElement(BlankTextElemName);
            writer.WriteRaw(BlankText);
            writer.WriteEndElement();
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (_blankText != null) hashCode ^= _blankText.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            FillBlankQuestion q = (FillBlankQuestion)obj;

            if (BlankText != q.BlankText) return false;

            return true;
        }

        public override object Clone()
        {
            return new FillBlankQuestion(this);
        }

        #endregion
    }
}
