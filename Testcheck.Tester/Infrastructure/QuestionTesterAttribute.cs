using System;


namespace TAlex.Testcheck.Tester.Infrastructure
{
    public class QuestionTesterAttribute : Attribute
    {
        #region Properties

        public Type QuestionType { get; private set; }

        #endregion

        #region Constructors

        public QuestionTesterAttribute(Type questionType)
        {
            QuestionType = questionType;
        }

        #endregion
    }
}
