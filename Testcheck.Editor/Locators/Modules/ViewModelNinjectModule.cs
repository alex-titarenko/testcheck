using System;
using Ninject.Modules;
using TAlex.Testcheck.Editor.ViewModels;


namespace TAlex.Testcheck.Editor.Locators.Modules
{
    class ViewModelNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AboutViewModel>().ToSelf().InSingletonScope();
        }
    }
}
