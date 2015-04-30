using System;
using TAlex.License;


namespace TAlex.Testcheck.Editor.Services.Licensing
{
    internal class AppLicenseDataManager : LicFileLicenseDataManager
    {
        protected override byte[] IV
        {
            get { return new byte[] { 8, 8, 41, 77, 18, 56, 101, 30 }; }
        }

        protected override byte[] SK
        {
            get { return new byte[] { 1, 19, 109, 64, 5, 99, 137, 64 }; }
        }
    }
}
