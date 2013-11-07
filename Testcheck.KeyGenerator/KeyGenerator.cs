using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TAlex.Testcheck.KeyGenerator.Helpers;


namespace TAlex.Testcheck.KeyGenerator
{
    internal class TestcheckKeyGenerator : IKeyGenerator
    {
        #region Fields

        private static readonly Random _rnd = new Random();

        private const int RegKeyLength = 150;

        private static readonly byte[] SK = new byte[]
        {
            5, 65, 165, 11, 70, 15, 1, 0
        };

        private static readonly byte[] IV = new byte[]
        {
            55, 66, 101, 3, 4, 93, 222, 16
        };

        private static readonly string[] _secretKeys = new string[]
        {
            "elrRj8CvpLfK4RI0n4alSG59hQP9222WT8Vm0Jb5",
	        "qP2a2pdz0TO09aYhhTUuro3EDr769GBhTV4iaD8N",
	        "iy3IEflipxRSg5E8Tq2qeigMSnbCWwr6CH8vwSr4",
	        "5vWfD278hZ8FtjZZ2SY4F0b9YA328kVpdcLF7Fb9",
	        "7e4dEe14Ssl963Dar29gS126FeejW3nPfJhX3A7u",
	        "dBELL4t1w7db0iCuV00F1d4Fb4Lj46yzeTZx34V8",
	        "UxkScykhuN0zvycDl552QQl58h072Nv4Ip3eo5Fw",
	        "ml9jPYMSP5106VbR923f8a34jSWdlhHP4Q2S9ikI",
	        "iCt887e0yQq84ZYK94Te9kz94vHYm20qGP3Q4UF0",
	        "Ej1mGoqr72cLM2bc573Bp9Hx641xr4AnTSy4v26g",
	        "Wm5cW5SjnPS9d5Bs15W1D93aGnc1wg8UjVupj8oV",
	        "19g14S3k22XBChnNCd3qsGcWI0E1f6191qD9u5Kn",
	        "75g0eDUkQ8dtiH7PpvNJRS20VRCl03p2blkDpkdA",
	        "u7mo51RrCUj7g1KQmONaNtw8OT5MF0XwY93L353G",
	        "2mzAmku62lrQGs45s0a43Z0Q8AjBIZQM9mnXkXd9",
	        "Eyu5rs0WqGiERUfLufFlz6ljWKhC54WFGp6fvlYr",
	        "06tyWdp0HSG6b560m7t90XV41km8D58ATFwcCM80",
	        "c3UE426G99rkp201opYixhlklJ1bszIh80DN1HW7",
	        "M8og87OqzMU2F0kx84l8MTu5C6QWpiAEG5590j63",
	        "qq711eq1B32gn8935giU3irhC4ep7Tq92We06lu0"
        };

        #endregion

        #region IKeyGenerator Members

        public object Generate(IDictionary<string, string> inputs)
        {
            SHA512 sha = new SHA512Managed();

            string regName = inputs[LicenseData.RegistrationNameParam];
            if (regName.Length < 5)
            {
                throw new KeyGeneratorException("REGNAME must have at least 5 characters", ReturnCode.ERC_BAD_INPUT);
            }

            string regNameHash = CryptoHelper.SHA512Base64(regName, new UTF8Encoding());

            const int linHashStartIndex = 10;
            const int secretKeyStartIndex = 108;

            string regKey = StringHelper.GenerateRandomString(linHashStartIndex);
            regKey += regNameHash;
            regKey += StringHelper.GenerateRandomString(secretKeyStartIndex - regKey.Length);

            int secretKeyIndex = _rnd.Next(_secretKeys.Length);
            regKey += _secretKeys[secretKeyIndex];

            regKey += StringHelper.GenerateRandomString(RegKeyLength - regKey.Length);

            DESCryptoServiceProvider cipher = new DESCryptoServiceProvider();
            cipher.IV = IV;
            cipher.Key = SK;
            byte[] encRegKeyData = CryptoHelper.Encrypt(regKey, cipher);
            string encRegKey = Convert.ToBase64String(encRegKeyData);

            return encRegKey;
        }

        #endregion
    }
}
