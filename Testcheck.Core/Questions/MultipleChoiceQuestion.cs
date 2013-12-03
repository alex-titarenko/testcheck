using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TAlex.Testcheck.Core.Choices;
using TAlex.Testcheck.Core.Helpers;

namespace TAlex.Testcheck.Core.Questions
{
    /// <summary>
    /// Represents the multiple choice of the type of question.
    /// </summary>
    /// <remarks>
    /// Only one of several choices should be selected as an answer.
    /// </remarks>
    [Serializable]
    public class MultipleChoiceQuestion : ShuffledChoicesQuestion
    {
        #region Fields

        private const string ChoicesElemName = "Choices";
        private const string ChoiceElemName = "Choice";
        private const string AnswerElemName = "Answer";

        private int _answer = -1;

        [NonSerialized]
        private int? _actualAnswer;

        private List<string> _choices = new List<string>();

        #endregion

        #region Properties

        public List<string> Choices
        {
            get
            {
                return _choices;
            }
        }

        public int Answer
        {
            get
            {
                return _answer;
            }

            set
            {
                _answer = value;
            }
        }

        [XmlIgnore]
        public int? ActualAnswer
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
                return "Multiple choice";
            }
        }

        public override bool CanCheck
        {
            get
            {
                return ActualAnswer.HasValue;
            }
        }

        #endregion

        #region Constructors

        public MultipleChoiceQuestion()
        {
        }

        protected MultipleChoiceQuestion(MultipleChoiceQuestion question)
            : base(question)
        {
            int n = question._choices.Count;
            for (int i = 0; i < n; i++)
            {
                _choices.Add((string)question._choices[i].Clone());
            }

            _answer = question._answer;
        }

        #endregion

        #region Methods

        public override decimal Check()
        {
            return (ActualAnswer == Answer) ? Points : 0;
        }

        public override void Shuffle()
        {
            int[] indexes = Shuffles.GetRandomSequence(Choices.Count, ShuffleMode);
            Answer = Array.IndexOf(indexes, Answer);
            Shuffles.ReorderBySequence(Choices, indexes);
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            Answer = XmlConvert.ToInt32(element[AnswerElemName].InnerText);

            _choices.Clear();

            XmlElement choicesElem = element[ChoicesElemName];
            foreach (XmlElement choiceElem in choicesElem.ChildNodes)
            {
                if (choiceElem.Name == ChoiceElemName)
                {
                    _choices.Add(choiceElem.InnerText);
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteStartElement(ChoicesElemName);

            foreach (string choice in Choices)
            {
                writer.WriteElementString(ChoiceElemName, choice);
            }

            writer.WriteEndElement();

            writer.WriteElementString(AnswerElemName, Answer.ToString());
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            for (int i = 0; i < _choices.Count; i++)
            {
                hashCode ^= _choices[i].GetHashCode();
            }

            hashCode ^= _answer;

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            MultipleChoiceQuestion q = (MultipleChoiceQuestion)obj;

            if (_answer != q._answer) return false;

            if (_choices.Count != q._choices.Count) return false;

            for (int i = 0; i < _choices.Count; i++)
            {
                if (_choices[i] != q._choices[i])
                    return false;
            }

            return true;
        }

        public override object Clone()
        {
            return new MultipleChoiceQuestion(this);
        }

        #endregion
    }
}
