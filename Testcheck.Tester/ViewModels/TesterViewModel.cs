using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using TAlex.Testcheck.Core;
using TAlex.Testcheck.Core.Questions;
using TAlex.Testcheck.Tester.Reporting;
using TAlex.WPF.Mvvm;
using TAlex.WPF.Mvvm.Commands;


namespace TAlex.Testcheck.Tester.ViewModels
{
    public class TesterViewModel : ViewModelBase
    {
        #region Fields

        private Timer _timer;
        private DateTime _startTime;
        private TimeSpan? _timeLimit;
        private TimeSpan _timeElapsed;
        private TimeSpan? _timeLeft; 

        private Question _currentQuestion;
        private int _currentQuestionNumber;
        private decimal _scoredPoints;
        private decimal _possiblePoints;

        private Dictionary<int, Question> _questions;
        private int _currQuestionIndex;

        private decimal _gradingScale;
        private UserInfo _userInfo;

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
                return _scoredPoints;
            }

            private set
            {
                Set(() => ScoredPoints, ref _scoredPoints, value);
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

        public TimeSpan? TimeLimit
        {
            get
            {
                return _timeLimit;
            }
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                return _timeElapsed;
            }

            private set
            {
                Set(() => TimeElapsed, ref _timeElapsed, value);
            }
        }

        public TimeSpan? TimeLeft
        {
            get
            {
                return _timeLeft;
            }

            private set
            {
                Set(() => TimeLeft, ref _timeLeft, value);
            }
        }

        #endregion

        #region Commands

        public ICommand AcceptCommand { get; set; }

        public ICommand MovePreviousCommand { get; set; }

        public ICommand MoveNextCommand { get; set; }

        #endregion

        #region Constructors

        public TesterViewModel(Test test, UserInfo userInfo)
        {
            _timer = new Timer(200);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            InitCommands();
            Init(test, userInfo);
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
                ShowTestResult();
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


        private void Init(Test test, UserInfo userInfo)
        {
            _userInfo = userInfo;
            _gradingScale = test.GradingScale;
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

                _timeLimit = test.NoTimelimit ? (TimeSpan?)null : test.Timelimit;
                _startTime = DateTime.Now;
                _timer.Start();
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


        private void ShowTestResult()
        {
            _timer.Stop();

            TestReport report = new TestReport
            {
                UserInfo = _userInfo,
                GradingScale = _gradingScale,
                TotalQuestion = QuestionsCount,
                AnsweredQuestion = QuestionsCount - _questions.Count,
                TotalPoints = TotalPoints,
                ScoredPoints = ScoredPoints,
                TimeElapsed = TimeElapsed
            };

            TAlex.Testcheck.Tester.Views.ResultWindow window = new TAlex.Testcheck.Tester.Views.ResultWindow(report);
            window.ShowDialog();
        }

        #region Event Handlers

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeElapsed = DateTime.Now - _startTime;
            TimeLeft = _timeLimit - TimeElapsed;

            if (TimeLeft.HasValue && TimeLeft <= TimeSpan.Zero)
            {
                ShowTestResult();
            }
        }

        #endregion

        #endregion
    }
}
