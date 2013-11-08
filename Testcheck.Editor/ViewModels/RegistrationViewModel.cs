using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TAlex.Common.Licensing;
using TAlex.Testcheck.Editor.Properties;
using TAlex.Testcheck.Editor.Services.Windows;
using TAlex.WPF.Mvvm.Commands;
using TAlex.WPF.Mvvm.Services;


namespace TAlex.Testcheck.Editor.ViewModels
{
    public class RegistrationViewModel
    {
        #region Fields

        protected readonly ILicenseDataManager LicenseDataManager;
        protected readonly IMessageService MessageService;
        protected readonly IApplicationService ApplicationService;

        #endregion

        #region Properties

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string LicenseName { get; set; }

        [Required]
        public string LicenseKey { get; set; }

        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructors

        public RegistrationViewModel(ILicenseDataManager licenseDataManager, IMessageService messageService, IApplicationService applicationService)
        {
            LicenseDataManager = licenseDataManager;
            MessageService = messageService;
            ApplicationService = applicationService;

            RegisterCommand = new RelayCommand(RegisterCommandExecute, RegisterCommandCanExecute);
        }

        #endregion

        #region Methods

        private void RegisterCommandExecute()
        {
            LicenseDataManager.Save(new LicenseData { LicenseName = LicenseName.Trim(), LicenseKey = LicenseKey.Trim() });
            MessageService.ShowInformation(Resources.locPleaseRestartToVerifyLicense, Resources.locInformationMessageCaption);
            ApplicationService.Shutdown();
        }

        private bool RegisterCommandCanExecute()
        {
            return !String.IsNullOrWhiteSpace(LicenseName) && !String.IsNullOrWhiteSpace(LicenseKey);
        }

        #endregion
    }
}
