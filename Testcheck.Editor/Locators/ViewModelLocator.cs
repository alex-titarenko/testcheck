using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAlex.Testcheck.Editor.Locators.Modules;
using TAlex.Testcheck.Editor.ViewModels;


namespace TAlex.Testcheck.Editor.Locators
{
    public class ViewModelLocator
    {
        #region Fields

        private IKernel _kernel;

        #endregion

        #region Constructors

        public ViewModelLocator()
        {
            _kernel = new StandardKernel(
                new BaseServicesNinjectModule(),
                new AppLicenseNinjectModule(),
                new ViewModelNinjectModule());
        }

        #endregion

        #region Properties

        public AboutViewModel AboutViewModel
        {
            get
            {
                return _kernel.Get<AboutViewModel>();
            }
        }

        public RegistrationViewModel RegistrationViewModel
        {
            get
            {
                return _kernel.Get<RegistrationViewModel>();
            }
        }

        #endregion

        #region Methods

        #region Methods

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }

        #endregion

        #endregion
    }
}
