using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAlex.Common.Environment;

namespace TAlex.Testcheck.Editor.Locators.Modules
{
    class BaseServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationInfo>().ToConstant(ApplicationInfo.Current).InSingletonScope();

            //Bind<IMessageService>().To<MessageService>();
            //Bind<IApplicationService>().To<ApplicationService>();
            //Bind<IRegistrationWindowService>().To<RegistrationWindowService>();
        }
    }
}
