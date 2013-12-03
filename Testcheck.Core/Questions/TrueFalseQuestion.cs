using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TAlex.Testcheck.Core.Questions
{
    /// <summary>
    /// Represents the true/false of the type of question.
    /// </summary>
    /// <remarks>
    /// Any given statement should be either confirmed or denied.
    /// </remarks>
    [Serializable]
    public class TrueFalseQuestion : Question
    {
        #region Fields

        private const string CorrectlyElemName = "Correctly";

        private bool _correctly;

        [NonSerialized]
        private bool? _actualChoice;

        #endregion

        #region Properties

        public bool Correctly
        {
            get
            {
                return _correctly;
            }

            set
            {
                _correctly = value;
            }
        }

        [XmlIgnore]
        public bool? ActualChoice
        {
            get
            {
                return _actualChoice;
            }

            set
            {
                _actualChoice = value;
            }
        }

        public override string TypeName
        {
            get
            {
                return "True/False";
            }
        }

        public override bool CanCheck
        {
            get
            {
                return ActualChoice.HasValue;
            }
        }

        #endregion

        #region Constructors

        public TrueFalseQuestion()
        {
        }

        protected TrueFalseQuestion(TrueFalseQuestion question)
            : base(question)
        {
            _correctly = question._correctly;
        }

        #endregion

        #region Methods

        public override decimal Check()
        {
            return (Correctly == ActualChoice) ? Points : 0;
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            Correctly = bool.Parse(element[CorrectlyElemName].InnerText);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteElementString(CorrectlyElemName, Correctly.ToString());
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            hashCode ^= _correctly.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            TrueFalseQuestion q = (TrueFalseQuestion)obj;

            if (Correctly != q.Correctly) return false;

            return true;
        }

        public override object Clone()
        {
            return new TrueFalseQuestion(this);
        }

        #endregion
    }
}
