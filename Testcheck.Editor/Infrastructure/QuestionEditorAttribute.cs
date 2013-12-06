using System;


namespace TAlex.Testcheck.Editor.Infrastructure
{
    public class QuestionEditorAttribute : Attribute
    {
        #region Properties

        public Type QuestionType { get; private set; }

        #endregion

        #region Constructors

        public QuestionEditorAttribute(Type questionType)
        {
            QuestionType = questionType;
        }

        #endregion
    }
}
