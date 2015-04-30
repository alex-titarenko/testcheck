using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAlex.Testcheck.Editor.Services.Windows;
using TAlex.Common.Models;
using TAlex.Mvvm.Services;
using System.Reflection;


namespace TAlex.Testcheck.Editor.Locators.Modules
{
    class BaseServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AssemblyInfo>().ToConstant(new AssemblyInfo(Assembly.GetExecutingAssembly())).InSingletonScope();

            Bind<IMessageService>().To<MessageService>();
            Bind<IApplicationService>().To<ApplicationService>();
        }
    }
}
