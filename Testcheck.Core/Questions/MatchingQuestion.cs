using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TAlex.Testcheck.Core.Helpers;

namespace TAlex.Testcheck.Core.Questions
{
    /// <summary>
    /// Represents the matching of the type of question.
    /// </summary>
    /// <remarks>
    /// There are two lists of choices.
    /// For choices in one list a match should be found from another list.
    /// </remarks>
    [Serializable]
    public class MatchingQuestion : ShuffledChoicesQuestion
    {
        #region Fields

        private const string LeftChoicesElemName = "LeftChoices";
        private const string RightChoicesElemName = "RightChoices";
        private const string ChoiceElemName = "Choice";
        private const string KeyPairsElemName = "KeyPairs";
        private const string KeyPairElemName = "KeyPair";
        private const string Key1AttrName = "Key1";
        private const string Key2AttrName = "Key2";
        private const string MatchingModeElemName = "MatchingMode";

        private List<string> _leftChoices = new List<string>();

        private List<string> _rightChoices = new List<string>();

        private List<KeyPair> _keyPairs = new List<KeyPair>();

        [NonSerialized]
        private ObservableCollection<KeyPair> _actualKeyPairs = new ObservableCollection<KeyPair>();

        private MatchingMode _matchingMode = MatchingMode.OneToOne;

        #endregion

        #region Properties

        public List<string> LeftChoices
        {
            get
            {
                return _leftChoices;
            }
        }

        public List<string> RightChoices
        {
            get
            {
                return _rightChoices;
            }
        }

        public List<KeyPair> KeyPairs
        {
            get
            {
                return _keyPairs;
            }
        }

        [XmlIgnore]
        public ObservableCollection<KeyPair> ActualKeyPairs
        {
            get
            {
                return _actualKeyPairs;
            }
        }

        public MatchingMode MatchingMode
        {
            get
            {
                return _matchingMode;
            }

            set
            {
                _matchingMode = value;
            }
        }

        public override string TypeName
        {
            get
            {
                return "Matching";
            }
        }

        public override bool CanCheck
        {
            get
            {
                return ActualKeyPairs != null && ActualKeyPairs.Count >= Math.Max(LeftChoices.Count, RightChoices.Count);
            }
        }

        #endregion

        #region Constructors

        public MatchingQuestion()
        {
            Init();
        }

        protected MatchingQuestion(MatchingQuestion question)
            : base(question)
        {
            Init();

            int nl = question._leftChoices.Count;
            for (int i = 0; i < nl; i++)
            {
                _leftChoices.Add((string)question._leftChoices[i].Clone());
            }

            int nr = question._rightChoices.Count;
            for (int i = 0; i < nr; i++)
            {
                _rightChoices.Add((string)question._rightChoices[i].Clone());
            }

            int nk = question._keyPairs.Count;
            for (int i = 0; i < nk; i++)
            {
                _keyPairs.Add(question._keyPairs[i]);
            }

            _matchingMode = question._matchingMode;
        }

        #endregion

        #region Methods

        public override decimal Check()
        {
            decimal pointValuePerPair = 1M / KeyPairs.Count;

            decimal pointValue = 0;

            foreach (KeyPair actualKeyPair in ActualKeyPairs)
            {
                if (_keyPairs.Contains(actualKeyPair))
                    pointValue += pointValuePerPair;
            }

            return pointValue * Points;
        }

        public override void Shuffle()
        {
            int[] leftSequence = Shuffles.GetRandomSequence(LeftChoices.Count, ShuffleMode);
            int[] rightSequence = Shuffles.GetRandomSequence(RightChoices.Count, ShuffleMode);

            for (int i = 0; i < KeyPairs.Count; i++)
            {
                KeyPair pair = _keyPairs[i];
                _keyPairs[i] = new KeyPair(Array.IndexOf(leftSequence, pair.Key1), Array.IndexOf(rightSequence, pair.Key2));
            }

            Shuffles.ReorderBySequence(LeftChoices, leftSequence);
            Shuffles.ReorderBySequence(RightChoices, rightSequence);
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);


            XmlElement leftChoicesElem = element[LeftChoicesElemName];

            _leftChoices.Clear();
            foreach (XmlElement item in leftChoicesElem.ChildNodes)
            {
                if (item.Name == ChoiceElemName)
                {
                    _leftChoices.Add(item.InnerText);
                }
            }

            XmlElement rightChoicesElem = element[RightChoicesElemName];

            _rightChoices.Clear();
            foreach (XmlElement item in rightChoicesElem.ChildNodes)
            {
                if (item.Name == ChoiceElemName)
                {
                    _rightChoices.Add(item.InnerText);
                }
            }

            XmlElement keyPairsElem = element[KeyPairsElemName];

            _keyPairs.Clear();
            foreach (XmlElement pair in keyPairsElem)
            {
                if (pair.Name == KeyPairElemName)
                {
                    _keyPairs.Add(new KeyPair(XmlConvert.ToInt32(pair.Attributes[Key1AttrName].InnerText),
                        XmlConvert.ToInt32(pair.Attributes[Key2AttrName].InnerText)));
                }
            }

            XmlElement matchingModeElem = element[MatchingModeElemName];
            if (matchingModeElem != null)
                MatchingMode = (MatchingMode)Enum.Parse(typeof(MatchingMode), matchingModeElem.InnerText);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);


            writer.WriteStartElement(LeftChoicesElemName);

            foreach (string item in LeftChoices)
            {
                writer.WriteElementString(ChoiceElemName, item);
            }

            writer.WriteEndElement();

            writer.WriteStartElement(RightChoicesElemName);

            foreach (string item in RightChoices)
            {
                writer.WriteElementString(ChoiceElemName, item);
            }

            writer.WriteEndElement();

            writer.WriteStartElement(KeyPairsElemName);

            foreach (KeyPair pair in KeyPairs)
            {
                writer.WriteStartElement(KeyPairElemName);
                writer.WriteAttributeString(Key1AttrName, pair.Key1.ToString());
                writer.WriteAttributeString(Key2AttrName, pair.Key2.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteElementString(MatchingModeElemName, MatchingMode.ToString());
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            for (int i = 0; i < _leftChoices.Count; i++)
            {
                hashCode ^= _leftChoices[i].GetHashCode();
            }

            for (int i = 0; i < _rightChoices.Count; i++)
            {
                hashCode ^= _rightChoices[i].GetHashCode();
            }

            for (int i = 0; i < _keyPairs.Count; i++)
            {
                hashCode ^= _keyPairs[i].GetHashCode();
            }

            hashCode ^= MatchingMode.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            MatchingQuestion q = (MatchingQuestion)obj;

            if (_matchingMode != q._matchingMode) return false;

            if (_leftChoices.Count != q._leftChoices.Count) return false;
            for (int i = 0; i < _leftChoices.Count; i++)
            {
                if (_leftChoices[i] != q._leftChoices[i])
                    return false;
            }

            if (_rightChoices.Count != q._rightChoices.Count) return false;
            for (int i = 0; i < _rightChoices.Count; i++)
            {
                if (_rightChoices[i] != q._rightChoices[i])
                    return false;
            }

            if (_keyPairs.Count != q._keyPairs.Count) return false;
            for (int i = 0; i < _keyPairs.Count; i++)
            {
                if (!KeyPair.Equals(_keyPairs[i], q._keyPairs[i]))
                    return false;
            }

            return true;
        }

        public override object Clone()
        {
            return new MatchingQuestion(this);
        }

        protected virtual void Init()
        {
            _actualKeyPairs = new ObservableCollection<KeyPair>();
            _actualKeyPairs.CollectionChanged += (s, a) => { OnCanCheckChanged(); };
        }

        #endregion
    }
}
