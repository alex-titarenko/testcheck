using System;
using TAlex.Common.Models;


namespace TAlex.Testcheck.Editor.ViewModels
{
    public class AboutViewModel
    {
        #region Fields

        public virtual AssemblyInfo AssemblyInfo { get; set; }

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

        #endregion

        #region Constructors

        public AboutViewModel(AssemblyInfo assemblyInfo)
        {
            AssemblyInfo = assemblyInfo;
        }

        #endregion
    }
}
