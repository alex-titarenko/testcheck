using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;


namespace TAlex.Testcheck.Core.Questions
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

        [NonSerialized]
        private List<string> _actualAnswers = new List<string>();

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

        [XmlIgnore]
        public List<string> ActualAnswers
        {
            get
            {
                return _actualAnswers;
            }
        }

        public override string TypeName
        {
            get
            {
                return "Fill in the Blank";
            }
        }

        public override bool CanCheck
        {
            get
            {
                IList<string> gaps = ExtractBlankGaps();
                return gaps.Count == ActualAnswers.Count && ActualAnswers.All(x => !String.IsNullOrWhiteSpace(x));
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
            BlankText = question.BlankText;
        }

        #endregion

        #region Methods

        public override decimal Check()
        {
            IList<string> gaps = ExtractBlankGaps();
            int count = ActualAnswers.Count;

            if (gaps.Count != count)
                throw new ArgumentOutOfRangeException();

            decimal pointValue = 0;
            decimal pointValuePerAnswer = 1M / count;

            for (int i = 0; i < count; i++)
            {
                if (gaps[i].ToUpper() == ActualAnswers[i].ToUpper())
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


        #region Helpers

        private IList<string> ExtractBlankGaps()
        {
            MatchCollection matches = Regex.Matches(_blankText, FieldPattern);
            return matches.Cast<Match>().Select(m => m.Groups["field"].Value).ToList();
        }

        #endregion

        #endregion
    }
}
