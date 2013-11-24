using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TAlex.Testcheck.Core.Questions
{
    /// <summary>
    /// Represents the essay of the type of question.
    /// </summary>
    /// <remarks>
    /// Participants should submit a free-range text of certain length.
    /// </remarks>
    [Serializable]
    public class EssayQuestion : Question
    {
        #region Fields

        private const string CorrectAnswersElemName = "CorrectAnswers";
        private const string AnswerElemName = "Answer";

        private List<string> _correctAnswers = new List<string>();

        [NonSerialized]
        private string _actualAnswer;

        #endregion

        #region Properties

        public List<string> CorrectAnswers
        {
            get
            {
                return _correctAnswers;
            }
        }

        [XmlIgnore]
        public string ActualAnswer
        {
            get
            {
                return _actualAnswer;
            }

            set
            {
                _actualAnswer = value;
            }
        }

        public override string TypeName
        {
            get
            {
                return "Essay";
            }
        }

        #endregion

        #region Constructors

        public EssayQuestion()
        {
        }

        protected EssayQuestion(EssayQuestion question)
            : base(question)
        {
            int n = question._correctAnswers.Count;

            for (int i = 0; i < n; i++)
            {
                _correctAnswers.Add((string)question._correctAnswers[i].Clone());
            }
        }

        #endregion

        #region Methods

        public override decimal Check()
        {
            string answer = ActualAnswer.ToUpper();
            return CorrectAnswers.Any(x => String.Equals(answer, x.ToUpper())) ? Points : 0;
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            XmlElement answers = element[CorrectAnswersElemName];

            _correctAnswers.Clear();
            foreach (XmlElement answer in answers.ChildNodes)
            {
                if (answer.Name == AnswerElemName)
                {
                    _correctAnswers.Add(answer.InnerText);
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteStartElement(CorrectAnswersElemName);

            foreach (string answer in CorrectAnswers)
            {
                writer.WriteElementString(AnswerElemName, answer);
            }

            writer.WriteEndElement();
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            for (int i = 0; i < _correctAnswers.Count; i++)
            {
                hashCode ^= _correctAnswers[i].GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            EssayQuestion q = (EssayQuestion)obj;

            if (_correctAnswers.Count != q._correctAnswers.Count) return false;

            for (int i = 0; i < _correctAnswers.Count; i++)
            {
                if (_correctAnswers[i] != q._correctAnswers[i])
                    return false;
            }

            return true;
        }

        public override object Clone()
        {
            return new EssayQuestion(this);
        }

        #endregion
    }
}
