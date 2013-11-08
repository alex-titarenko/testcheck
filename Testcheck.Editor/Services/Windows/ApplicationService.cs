using System;
using System.Windows;


namespace TAlex.Testcheck.Editor.Services.Windows
{
    public class ApplicationService : IApplicationService
    {
        #region IApplicationService Members

        public void Shutdown()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
