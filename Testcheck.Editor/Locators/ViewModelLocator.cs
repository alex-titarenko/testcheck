using Ninject;
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
