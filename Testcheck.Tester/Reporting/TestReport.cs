using System;
using TAlex.Testcheck.Core;


namespace TAlex.Testcheck.Tester.Reporting
{
    public class TestReport
    {
        public UserInfo UserInfo
        {
            get;
            set;
        }

        public int TotalQuestion
        {
            get;
            set;
        }

        public int AnsweredQuestion
        {
            get;
            set;
        }

        public decimal ScoredPoints
        {
            get;
            set;
        }

        public decimal TotalPoints
        {
            get;
            set;
        }

        public decimal GradingScale
        {
            get;
            set;
        }

        public decimal PercentCorrect
        {
            get
            {
                if (TotalQuestion != 0)
                    return ScoredPoints / TotalPoints;
                else
                    return 0;
            }
        }

        public decimal Grade
        {
            get
            {
                return PercentCorrect * GradingScale;
            }
        }

        public TimeSpan TimeElapsed
        {
            get;
            set;
        }
    }
}
