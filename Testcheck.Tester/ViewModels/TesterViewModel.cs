using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TAlex.Testcheck.Core;
using TAlex.Testcheck.Core.Questions;
using TAlex.WPF.Mvvm;
using TAlex.WPF.Mvvm.Commands;


namespace TAlex.Testcheck.Tester.ViewModels
{
    public class TesterViewModel : ViewModelBase
    {
        #region Fields

        private Question _currentQuestion;
        private int _currentQuestionNumber;
        private decimal _points;
        private decimal _possiblePoints;

        private Dictionary<int, Question> _questions;
        private int _currQuestionIndex;

        #endregion

        #region Properties

        public Question CurrentQuestion
        {
            get
            {
                return _currentQuestion;
            }

            private set
            {
                Set(() => CurrentQuestion, ref _currentQuestion, value);
            }
        }

        public int CurrentQuestionNumber
        {
            get
            {
                return _currentQuestionNumber;
            }

            private set
            {
                Set(() => CurrentQuestionNumber, ref _currentQuestionNumber, value);
            }
        }

        public int QuestionsCount
        {
            get; private set;
        }

        public decimal ScoredPoints
        {
            get
            {
                return _points;
            }

            private set
            {
                Set(() => ScoredPoints, ref _points, value);
            }
        }

        public decimal PossiblePoints
        {
            get
            {
                return _possiblePoints;
            }

            private set
            {
                Set(() => PossiblePoints, ref _possiblePoints, value);
            }
        }

        public decimal TotalPoints
        {
            get;
            private set;
        }

        #endregion

        #region Commands

        public ICommand AcceptCommand { get; set; }

        public ICommand MovePreviousCommand { get; set; }

        public ICommand MoveNextCommand { get; set; }

        #endregion

        #region Constructors

        public TesterViewModel(Test test)
        {
            InitCommands();
            Init(test);
        }

        #endregion

        #region Methods

        private void InitCommands()
        {
            AcceptCommand = new RelayCommand(AcceptCommandExecute, CanAccept);
            MovePreviousCommand = new RelayCommand(MovePreviousCommandExecute, CanMoveTo);
            MoveNextCommand = new RelayCommand(MoveNextCommandExecute, CanMoveTo);
        }

        private void AcceptCommandExecute()
        {
            ScoredPoints += CurrentQuestion.Check();
            PossiblePoints += CurrentQuestion.Points;

            _questions.Remove(CurrentQuestionNumber);
            _currQuestionIndex = (_currQuestionIndex >= _questions.Count) ? 0 : _currQuestionIndex;

            if (_questions.Any())
            {
                SetCurrentQuestion(_currQuestionIndex);
            }
            else
            {
                //ShowResultTest();
            }
        }

        private void MovePreviousCommandExecute()
        {
            _currQuestionIndex--;
            if (_currQuestionIndex < 0) _currQuestionIndex = _questions.Count - 1;
            SetCurrentQuestion(_currQuestionIndex);
        }

        private void MoveNextCommandExecute()
        {
            _currQuestionIndex++;
            if (_currQuestionIndex >= _questions.Count) _currQuestionIndex = 0;
            SetCurrentQuestion(_currQuestionIndex);
        }

        private bool CanAccept()
        {
            return _questions.Any();
        }

        private bool CanMoveTo()
        {
            return _questions.Count > 1;
        }


        private void Init(Test test)
        {
            _questions = new Dictionary<int, Question>();
            _currQuestionIndex = 0;

            QuestionsCount = test.QuestionCount;
            TotalPoints = test.TotalPoints;
            ScoredPoints = 0;

            for (int i = 0; i < test.QuestionCount; i++)
            {
                _questions.Add(i + 1, (Question)test.Questions[i].Clone());
            }

            if (_questions.Any())
            {
                SetCurrentQuestion(_currQuestionIndex);
            }
        }

        private void SetCurrentQuestion(int questionIndex)
        {
            CurrentQuestionNumber = GetQuestionNumberByIndex(questionIndex);
            CurrentQuestion = _questions[CurrentQuestionNumber];
        }

        private int GetQuestionNumberByIndex(int index)
        {
            foreach (KeyValuePair<int, Question> pair in _questions)
            {
                if (index == 0) return pair.Key;
                index--;
            }

            throw new IndexOutOfRangeException();
        }

        #endregion
    }
}
