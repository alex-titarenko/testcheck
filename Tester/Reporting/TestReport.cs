using System;
using System.Collections.Generic;
using System.Text;

namespace TAlex.Testcheck.Tester.Reporting
{
    public class TestReport
    {
        #region Fields

        private string _userName;

        private string _userGroup;

        private int _totalQuestion;

        private int _answeredQuestion;

        private decimal _points;

        private decimal _totalPoints;

        private decimal _gradingScale;

        private TimeSpan _timeElapsed;

        #endregion

        #region Properties

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }

        public string UserGroup
        {
            get
            {
                return _userGroup;
            }

            set
            {
                _userGroup = value;
            }
        }

        public int TotalQuestion
        {
            get
            {
                return _totalQuestion;
            }

            set
            {
                _totalQuestion = value;
            }
        }

        public int AnsweredQuestion
        {
            get
            {
                return _answeredQuestion;
            }

            set
            {
                _answeredQuestion = value;
            }
        }

        public decimal Points
        {
            get
            {
                return _points;
            }

            set
            {
                _points = value;
            }
        }

        public decimal TotalPoints
        {
            get
            {
                return _totalPoints;
            }

            set
            {
                _totalPoints = value;
            }
        }

        public decimal GradingScale
        {
            get
            {
                return _gradingScale;
            }

            set
            {
                _gradingScale = value;
            }
        }

        public decimal PercentCorrect
        {
            get
            {
                if (_totalQuestion != 0)
                    return _points / _totalPoints;
                else
                    return 0;
            }
        }

        public decimal Grade
        {
            get
            {
                return PercentCorrect * _gradingScale;
            }
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                return _timeElapsed;
            }

            set
            {
                _timeElapsed = value;
            }
        }

        #endregion
    }
}
