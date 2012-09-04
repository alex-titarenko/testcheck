using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TAlex.Testcheck.Tester.TestCore.Questions
{
    /// <summary>
    /// Provides a base class for different types of questions
    /// that support shuffling of choices,
    /// such as multiple choice, ranking, matching, etc.
    /// </summary>
    [Serializable]
    public abstract class ShuffledChoicesQuestion : Question, IShuffles
    {
        #region Fields

        private const string ShuffleModeElemName = "ShuffleMode";

        #endregion

        #region Properties

        public ShuffleMode ShuffleMode { get; set; }

        #endregion

        #region Constructors

        public ShuffledChoicesQuestion()
        {
        }

        protected ShuffledChoicesQuestion(ShuffledChoicesQuestion question)
            : base(question)
        {
            ShuffleMode = question.ShuffleMode;
        }

        #endregion

        #region Methods

        protected override void ReadXml(System.Xml.XmlElement element)
        {
            base.ReadXml(element);

            XmlElement shuffleModeElem = element[ShuffleModeElemName];
            if (shuffleModeElem != null)
                ShuffleMode = (ShuffleMode)Enum.Parse(typeof(ShuffleMode), shuffleModeElem.InnerText);
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteElementString(ShuffleModeElemName, ShuffleMode.ToString());
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            hashCode ^= ShuffleMode.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            ShuffledChoicesQuestion q = (ShuffledChoicesQuestion)obj;

            if (ShuffleMode != q.ShuffleMode) return false;

            return true;
        }

        #endregion
    }
}
