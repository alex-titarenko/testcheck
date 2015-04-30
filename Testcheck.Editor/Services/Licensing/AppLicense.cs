using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAlex.License;


namespace TAlex.Testcheck.Editor.Services.Licensing
{
    internal class AppLicense : LicenseBase
    {
        #region Fields

        private static readonly byte[] _IV = new byte[]
        {
            55, 66, 101, 3, 4, 93, 222, 16
        };

        private static readonly byte[] _SK = new byte[]
        {
            5, 65, 165, 11, 70, 15, 1, 0
        };

        private static readonly List<string> _SKH = new List<string>
        {
            "ecZO0z9XrjAVQIK0b7iR5MXx5twG1hBjoeH93vG1j0hxZYUiRgurOo0CAdUl28KCdqWgTPfdBtYRiPf/842zQw==",
	        "fnDtFm+5s66HnSXeBClUf1U/UJ3YWDF2uY30ShXqnCx9GNqs/Vk4mBw/xrh92kpBNEwIWKjEQa1yRKysfG+dxA==",
	        "iHcL2OZ2BrBtTWc/UNSTTa7b7yNedpwgV0DXRZwU2/fN1+4ANVwDgOLkYonV2eQwZvLUnex9IWinu2yetrUQ/A==",
	        "N3ncmsZlrS3bO31iAXOAYfQ2zhqA2+Gd9uGdErrjFWWkPAi3YtrOgrgkiKOKj7nbStAwPoA2CwbVBzBdpRR0tg==",
	        "/9QsvbWnzjsHqXv56Kn8lxvRJ60SBc1xPCWkZ+wD0gZWA/h/qvtC8umr8Q7QOwI+XYCb9sdP8G2xX22uohyBvA==",
	        "RrsK5f6sh8y5I+PjVNsUlqJFQImy6mHWpGJozxD+iVRV50FPA0RTNoHs3vm2ZSgS30piQT5MVSuDWCpZSFYypA==",
	        "+DR1OscXu3VA3fMsUpzX6CGG+u7J8ABmb42aSZ11+gYhuQxHR1/tsVTm4aLoCQf89Ew0G8D7QCb7SvQQFIvB0Q==",
	        "wHWa6dNaHkXCKjG8gRHX5Wpih7zbachhbWuKwrFtadxRqjSbmhHYfdqM/78AQS+e3MlN8TyLJuAvvc75KyQT3g==",
	        "ehmzXPopYgtmiytnc79q7hbP3YGKpIaJgE/6SLqnBo8OROZkutmndGfuZYfbOgHD1ZVujPKi6oEuKaARGaQawA==",
	        "UnZdgYITB+2erVv3cFrRPjCwKcxc7DgfxvCGVOTZghAyi0Z9QHD7kxrdWSr/pjnUvVfeTbSyWrTqA/Eqd7rIXQ==",
	        "hUGQ/QIQflRu7aTQDhyLeRmmWuQ2xP4k8FN+XSaFYG8Xm8fJzDE7P9OjKlAYhF+6LaDHuF4O2SPxAvfNFub0mg==",
	        "YM2jeel2T4UcHHnOXDazQJDX1MPNOMkJajf3iCU0ITwXgAQSFuKIelaurfsRsH7W1cRTGk78sNj2Og6pCKdTng==",
	        "zVl3GQEhRuh6qtj5mCa2oAK0Dm1EYVetzI75hXZHHI8NehKUyWwzAtYVlbMQDTCRV1RCt6+9+A4Ai5wD7cqeiQ==",
	        "qGY7OSsKCH39AHxAvQBJoiN6vKK6qnpbeai7F6bzBeMj46KZ0QNKpbr2VgN2NFVnPBrUx6ANhh7yWgYA5AZBWg==",
	        "WDABsjoiSzDewBI8Oh/mUidlirFNYPU+GJWXrHLbv+0iZo/WoU2XrLsas5lOtfyslEDgz4+iqcQTxihwkzmZug==",
	        "OJEqJ1DR8SLeIUYv7XAJTilE2BSSiRazccJ9OtI3MsUWbWNOtzR8VQIWUwLd8Lqr8XmVIG1HmOBIt03pS+rarw==",
	        "p26uSfZs4IFk47L2WaP1j/1614+aG7mlunelhyXQyBz4FBW1kKrn7ULAn9NLi+CsZFvAEPwJGnGwMEgFduo3XA==",
	        "riDYWeAG8oDCnGMQ4A4X2D1z3o+l0fgPQWoZp7+beHPZw+re8sDX4e/hbzDibpoKZc24AsdZ0yxRd9SH13ABSg==",
	        "feFGw8ZX6v1vc804qqtYK1/O8hDuxfCSTn+KkNdQZHYZ7unKA65sOCHphTtgIlD3kg94lY05QorsWz5JG9ZIrA==",
	        "qx9ueNtrsUzeYuoZH3+Dhv3Oigw//9Ei2o/VKSVNy4Tq/9Qa6wl9A5c6iAHRE6KqaVtjNFXniHsXDTyUEduZJw=="
        };

        #endregion

        #region Properties

        protected override byte[] IV
        {
            get
            {
                return _IV;
            }
        }

        protected override byte[] SK
        {
            get
            {
                return _SK;
            }
        }

        protected override List<string> SKH
        {
            get
            {
                return _SKH;
            }
        }

        #endregion

        #region Constructors

        public AppLicense(ILicenseDataManager licenseDataManager, ITrialPeriodDataProvider trialPeriodDataProvider)
            : base(licenseDataManager, trialPeriodDataProvider)
        {
        }

        #endregion
    }
}
