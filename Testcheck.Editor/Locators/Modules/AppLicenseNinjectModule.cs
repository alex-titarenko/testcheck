using Ninject.Modules;
using System;
using TAlex.Testcheck.Editor.Services.Licensing;
using TAlex.License;


namespace TAlex.Testcheck.Editor.Locators.Modules
{
    class AppLicenseNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILicenseDataManager>().To<AppLicenseDataManager>().InSingletonScope();
            Bind<ITrialPeriodDataProvider>().To<AppTrialPeriodDataProvider>().InSingletonScope();
            Bind<LicenseBase>().To<AppLicense>().InSingletonScope();
        }
    }
}
