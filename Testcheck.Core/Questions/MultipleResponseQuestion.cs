using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TAlex.Testcheck.Core.Choices;


namespace TAlex.Testcheck.Core.Questions
{
    /// <summary>
    /// Represents the multiple response of the type of question.
    /// </summary>
    /// <remarks>
    /// Selection is made out of several choices. However, more than one choice can make an answer.
    /// </remarks>
    [Serializable]
    public class MultipleResponseQuestion : ShuffledChoicesQuestion
    {
        #region Fields

        private const string ChoicesElemName = "Choices";
        private const string ChoiceElemName = "Choice";
        private const string IsCorrectAttrName = "IsCorrect";

        private List<AnswerChoice> _choices = new List<AnswerChoice>();

        #endregion

        #region Properties

        public List<AnswerChoice> Choices
        {
            get
            {
                return _choices;
            }
        }

        public override string TypeName
        {
            get
            {
                return "Multiple response";
            }
        }

        #endregion

        #region Constructors

        public MultipleResponseQuestion()
        {
        }

        protected MultipleResponseQuestion(MultipleResponseQuestion question)
            : base(question)
        {
            int n = question._choices.Count;
            for (int i = 0; i < n; i++)
            {
                _choices.Add(new AnswerChoice((string)question._choices[i].Description.Clone(), question._choices[i].IsCorrect));
            }
        }

        #endregion

        #region Methods

        public override decimal Check(object data)
        {
            int correctAnswers = _choices.Count(x => x.IsCorrect);
            
            decimal pointValuePerAnswer = 1M / correctAnswers;
            decimal pointValuePerError = 1M / (_choices.Count - correctAnswers);

            decimal pointValue = 0;

            foreach (AnswerChoice choice in _choices.Where(x => x.ActualChoice == true))
            {
                if (choice.IsCorrect)
                    pointValue += pointValuePerAnswer;
                else
                    pointValue -= pointValuePerError;
            }

            return pointValue * Points;
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            _choices.Clear();
            XmlElement choices = element[ChoicesElemName];

            foreach (XmlElement choice in choices.ChildNodes)
            {
                if (choice.Name == ChoiceElemName)
                {
                    AnswerChoice ans = new AnswerChoice();
                    XmlAttribute correctAttr = choice.Attributes[IsCorrectAttrName];
                    if (correctAttr != null)
                        ans.IsCorrect = bool.Parse(correctAttr.InnerText);

                    ans.Description = choice.InnerText;

                    _choices.Add(ans);
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteStartElement(ChoicesElemName);

            foreach (AnswerChoice choice in Choices)
            {
                writer.WriteStartElement(ChoiceElemName);
                if (choice.IsCorrect)
                    writer.WriteAttributeString(IsCorrectAttrName, choice.IsCorrect.ToString());
                writer.WriteString(choice.Description);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            for (int i = 0; i < _choices.Count; i++)
            {
                hashCode ^= _choices[i].GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            MultipleResponseQuestion q = (MultipleResponseQuestion)obj;

            if (_choices.Count != q._choices.Count) return false;

            for (int i = 0; i < _choices.Count; i++)
            {
                if (!AnswerChoice.Equals(_choices[i], q._choices[i]))
                    return false;
            }

            return true;
        }

        public override object Clone()
        {
            return new MultipleResponseQuestion(this);
        }

        #endregion
    }
}
