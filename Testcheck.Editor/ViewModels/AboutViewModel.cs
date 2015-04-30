using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TAlex.Common.Models;
using TAlex.License;


namespace TAlex.Testcheck.Editor.ViewModels
{
    public class AboutViewModel
    {
        #region Fields

        public virtual AssemblyInfo AssemblyInfo { get; set; }
        protected readonly LicenseBase AppLicense;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the email support title for this product.
        /// </summary>
        public virtual string EmailTitle
        {
            get
            {
                return EmailAddress.Replace("mailto:", String.Empty);
            }
        }

        /// <summary>
        /// Gets the email support for this product.
        /// </summary>
        public virtual string EmailAddress
        {
            get
            {
                return Properties.Resources.SupportEmail;
            }
        }

        /// <summary>
        /// Gets the homepage title of this product.
        /// </summary>
        public virtual string HomepageTitle
        {
            get
            {
                return HomepageUrl.Replace("http://", String.Empty);
            }
        }

        /// <summary>
        /// Gets the homepage url of this product.
        /// </summary>
        public virtual string HomepageUrl
        {
            get
            {
                return Properties.Resources.HomepageUrl;
            }
        }


        public virtual string LicenseName
        {
            get
            {
                return AppLicense.LicenseName;
            }
        }

        public virtual bool LicenseInfoVisibility
        {
            get
            {
                return AppLicense.IsLicensed;
            }
        }

        public bool UnregisteredTextVisibility
        {
            get
            {
                return !LicenseInfoVisibility;
            }
        }

        #endregion

        #region Constructors

        public AboutViewModel(AssemblyInfo assemblyInfo, LicenseBase appLicense)
        {
            AssemblyInfo = assemblyInfo;
            AppLicense = appLicense;
        }

        #endregion
    }
}
