using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Core.Helpers;


namespace TAlex.Testcheck.Core
{
    [Serializable]
    [XmlRoot("Test")]
    public class Test : ICloneable
    {
        #region Fields

        public const string XmlTestFileExtension = ".xml";
        public const string BinaryTestFileExtension = ".tst";

        private const string TitlePropertyName = "Title";
        private const string DescriptionPropertyName = "Description";
        private const string AuthorPropertyName = "Author";
        private const string CopyrightPropertyName = "Copyright";
        private const string TimelimitPropertyName = "Timelimit";
        private const string ShuffleQuestionsPropertyName = "ShuffleQuestions";
        private const string GradingScalePropertyName = "GradingScale";
        private const string QuestionsPropertyName = "Questions";


        private string _title = String.Empty;

        private string _description = String.Empty;

        private string _author = String.Empty;

        private string _copyright = String.Empty;

        private TimeSpan _timelimit = TimeSpan.FromMinutes(45);

        private bool _shuffleQuestions = true;

        private Decimal _gradingScale = 100;

        private string _password = String.Empty;

        private List<Question> _questions = new List<Question>();

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

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

        public string Author
        {
            get
            {
                return _author;
            }

            set
            {
                _author = value;
            }
        }

        public string Copyright
        {
            get
            {
                return _copyright;
            }

            set
            {
                _copyright = value;
            }
        }

        [XmlIgnore]
        public TimeSpan Timelimit
        {
            get
            {
                return _timelimit;
            }

            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException();

                _timelimit = value;
            }
        }

        [XmlElement(TimelimitPropertyName)]
        public string TimelimitString
        {
            get
            {
                return _timelimit.ToString();
            }

            set
            {
                _timelimit = TimeSpan.Parse(value);
            }
        }

        [XmlIgnore]
        public bool NoTimelimit
        {
            get
            {
                return (_timelimit == null) || (_timelimit == TimeSpan.Zero);
            }
        }

        public bool ShuffleQuestions
        {
            get
            {
                return _shuffleQuestions;
            }

            set
            {
                _shuffleQuestions = value;
            }
        }

        public Decimal GradingScale
        {
            get
            {
                return _gradingScale;
            }

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                _gradingScale = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }

        [XmlIgnore]
        public Decimal TotalPoints
        {
            get
            {
                decimal totalPoints = 0;
                foreach (Question question in _questions)
                {
                    totalPoints += question.Points;
                }

                return totalPoints;
            }
        }

        [XmlArray]
        [XmlArrayItem(typeof(TrueFalseQuestion))]
        [XmlArrayItem(typeof(MultipleChoiceQuestion))]
        [XmlArrayItem(typeof(MultipleResponseQuestion))]
        [XmlArrayItem(typeof(EssayQuestion))]
        [XmlArrayItem(typeof(FillBlankQuestion))]
        [XmlArrayItem(typeof(MatchingQuestion))]
        [XmlArrayItem(typeof(RankingQuestion))]
        public List<Question> Questions
        {
            get
            {
                return _questions;
            }
        }

        [XmlIgnore]
        public int QuestionCount
        {
            get
            {
                return _questions.Count;
            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public static Test Load(string path)
        {
            string extension = Path.GetExtension(path);
            FileStream file = new FileStream(path, FileMode.Open);
            Test test = null;

            switch (extension)
            {
                case BinaryTestFileExtension:
                    test = (Test)CryptoHelper.DecriptBinaryTestFile(file);
                    break;

                case XmlTestFileExtension:
                    XmlSerializer sr = new XmlSerializer(typeof(Test));
                    test = (Test)sr.Deserialize(file);
                    break;

                default:
                    throw new ArgumentException("Unknown file type.");
            }

            file.Close();

            return test;
        }

        public void Save(string path)
        {
            string extension = Path.GetExtension(path);

            if (String.IsNullOrEmpty(extension))
            {
                extension = BinaryTestFileExtension;
                path = Path.ChangeExtension(path, BinaryTestFileExtension);
            }

            FileStream file = new FileStream(path, FileMode.Create);

            switch (extension)
            {
                case XmlTestFileExtension:
                    XmlSerializer sr = new XmlSerializer(typeof(Test));
                    sr.Serialize(file, this);
                    break;

                case BinaryTestFileExtension:
                    CryptoHelper.EncryptBinaryTestFile(this, file);
                    break;

                default:
                    throw new ArgumentException();
            }

            file.Close();
        }

        public void Shuffle()
        {
            if (ShuffleQuestions)
            {
                Shuffles.Shuffle<Question>(Questions);
            }

            foreach (IShuffles question in Questions.OfType<IShuffles>())
            {
                question.Shuffle();
            }
        }

        public override int GetHashCode()
        {
            int hashCode;

            hashCode = _title.GetHashCode();
            hashCode ^= _description.GetHashCode();
            hashCode ^= _author.GetHashCode();
            hashCode ^= _copyright.GetHashCode();
            hashCode ^= _timelimit.GetHashCode();
            hashCode ^= _shuffleQuestions.GetHashCode();
            hashCode ^= _gradingScale.GetHashCode();
            hashCode ^= (_password + String.Empty).GetHashCode();

            foreach (Question question in Questions)
            {
                hashCode ^= question.GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Test t = (Test)obj;

            if (Title != t.Title) return false;
            if (Description != t.Description) return false;
            if (Author != t.Author) return false;
            if (Copyright != t.Copyright) return false;

            if (Timelimit != t.Timelimit) return false;
            if (ShuffleQuestions != t.ShuffleQuestions) return false;
            if (GradingScale != t.GradingScale) return false;
            if (Password != t.Password) return false;

            if (QuestionCount != t.QuestionCount) return false;
            for (int i = 0; i < QuestionCount; i++)
            {
                if (!Question.Equals(Questions[i], t.Questions[i]))
                    return false;
            }

            return true;
        }

        public object Clone()
        {
            Test test = new Test();
            test._title = (string)_title.Clone();
            test._description = (string)_description.Clone();
            test._author = (string)_author.Clone();
            test._copyright = (string)_copyright.Clone();
            test._timelimit = _timelimit;
            test._shuffleQuestions = _shuffleQuestions;
            test._gradingScale = _gradingScale;
            test._password = _password;

            for (int i = 0; i < QuestionCount; i++)
            {
                test._questions.Add((Question)_questions[i].Clone());
            }

            return test;
        }

        #endregion
    }
}
