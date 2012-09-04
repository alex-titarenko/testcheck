using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TAlex.Testcheck.Tester.TestCore.Questions
{
    /// <summary>
    /// Represents the ranking of the type of question.
    /// </summary>
    /// <remarks>
    /// A list of choices has certain logic.
    /// The logical order of the sequence should be restored.
    /// </remarks>
    [Serializable]
    public class RankingQuestion : ShuffledChoicesQuestion
    {
        #region Fields

        private const string ChoicesElemName = "Choices";
        private const string ChoiceElemName = "Choice";

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

        public override string TypeName
        {
            get
            {
                return "Ranking";
            }
        }

        #endregion

        #region Constructors

        public RankingQuestion()
        {
        }

        protected RankingQuestion(RankingQuestion question)
            : base(question)
        {
            int n = question._choices.Count;
            for (int i = 0; i < n; i++)
            {
                _choices.Add((string)question._choices[i].Clone());
            }
        }

        #endregion

        #region Methods

        public override decimal Check(object data)
        {
            return Check(data as int[]);
        }

        public decimal Check(int[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] != i)
                    return 0;
            }

            return Points;
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            _choices.Clear();

            XmlElement choicesElem = element[ChoicesElemName];
            foreach (XmlElement choice in choicesElem.ChildNodes)
            {
                if (choice.Name == ChoiceElemName)
                {
                    _choices.Add(choice.InnerText);
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

            RankingQuestion q = (RankingQuestion)obj;

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
            return new RankingQuestion(this);
        }

        #endregion
    }
}
