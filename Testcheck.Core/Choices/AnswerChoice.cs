using System;
using System.Xml.Serialization;

namespace TAlex.Testcheck.Core.Choices
{
    [Serializable]
    public class AnswerChoice
    {
        #region Fields

        private string _description = String.Empty;

        private bool _isCorrect = false;

        [NonSerialized]
        private bool _actualChoice;

        #endregion

        #region Properties

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public bool IsCorrect
        {
            get
            {
                return _isCorrect;
            }

            set
            {
                _isCorrect = value;
            }
        }

        [XmlIgnore]
        public bool ActualChoice
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

        #endregion

        #region Constructors

        public AnswerChoice()
        {
        }

        public AnswerChoice(string description, bool isCorrect)
        {
            _description = description;
            _isCorrect = isCorrect;
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            return _description.GetHashCode() ^ _isCorrect.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            
            AnswerChoice ans = (AnswerChoice)obj;

            if (_description != ans._description) return false;
            if (_isCorrect != ans._isCorrect) return false;

            return true;
        }

        #endregion
    }
}
