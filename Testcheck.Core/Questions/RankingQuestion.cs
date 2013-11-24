using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TAlex.Testcheck.Core.Helpers;

namespace TAlex.Testcheck.Core.Questions
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

        private List<RankingChoice> _choices = new List<RankingChoice>();

        #endregion

        #region Properties

        public List<RankingChoice> Choices
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
            foreach (var choice in question.Choices)
            {
                _choices.Add(new RankingChoice(choice.Order, choice.Choice));
            }
        }

        #endregion

        #region Methods

        public override decimal Check()
        {
            for (int i = 0; i < Choices.Count - 1; i++)
            {
                if (Choices[i].Order >= Choices[i + 1].Order)
                    return 0;
            }
            return Points;
        }

        public override void Shuffle()
        {
            Shuffles.Shuffle(Choices, ShuffleMode);
        }

        public void AddChoice(string choice)
        {
            _choices.Add(new RankingChoice(_choices.Any() ? _choices.Max(x => x.Order) + 1 : 0, choice));
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            _choices.Clear();

            XmlElement choicesElem = element[ChoicesElemName];
            int order = 0;
            foreach (XmlElement choice in choicesElem.ChildNodes)
            {
                if (choice.Name == ChoiceElemName)
                {
                    _choices.Add(new RankingChoice(order++, choice.InnerText));
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteStartElement(ChoicesElemName);

            foreach (RankingChoice choice in Choices.OrderBy(x => x.Order))
            {
                writer.WriteElementString(ChoiceElemName, choice.Choice);
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

        #region Nested Types

        [Serializable]
        public class RankingChoice
        {
            #region Fields

            private int _order;

            private string _choice;

            #endregion

            #region Properties
            
            public int Order
            {
                get
                {
                    return _order;
                }

                set
                {
                    _order = value;
                }
            }

            public string Choice
            {
                get
                {
                    return _choice;
                }

                set
                {
                    _choice = value;
                }
            }

            #endregion

            #region Construcrors

            public RankingChoice()
            {
            }

            public RankingChoice(int order, string choice)
                : this()
            {
                Order = order;
                Choice = choice;
            }

            #endregion
        }

        #endregion
    }
}
