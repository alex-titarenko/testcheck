using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;


namespace TAlex.Testcheck.Core.Helpers
{
    public static class CryptoHelper
    {
        #region Fields

        private static byte[] SK = new byte[]
        {
            164, 12, 1, 13, 76, 14, 95, 87
        };

        private static byte[] IV = new byte[]
        {
            24, 188, 157, 28, 138, 4, 126, 18
        };

        public static SymmetricAlgorithm Cipher;

        #endregion

        #region Constructors

        static CryptoHelper()
        {
            Cipher = new DESCryptoServiceProvider();
            Cipher.IV = IV;
            Cipher.Key = SK;
        }

        #endregion

        #region Methods

        public static byte[] Encrypt(string plaintext, SymmetricAlgorithm cipher)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, cipher.CreateEncryptor(), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(encStream);
            sw.WriteLine(plaintext);

            sw.Close();
            encStream.Close();

            byte[] buffer = ms.ToArray();

            ms.Close();
            return buffer;
        }

        public static string Decript(byte[] ciphertext, SymmetricAlgorithm cipher)
        {
            MemoryStream ms = new MemoryStream(ciphertext);
            CryptoStream decStream = new CryptoStream(ms, cipher.CreateDecryptor(), CryptoStreamMode.Read);

            StreamReader sr = new StreamReader(decStream);
            string result = sr.ReadToEnd();

            sr.Close();
            decStream.Close();
            ms.Close();

            return result;
        }


        public static void EncryptBinaryTestFile(object obj, Stream stream)
        {
            CryptoStream encStream = new CryptoStream(stream, Cipher.CreateEncryptor(), CryptoStreamMode.Write);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(encStream, obj);

            encStream.Close();
            stream.Close();
        }

        public static object DecriptBinaryTestFile(Stream stream)
        {
            CryptoStream decStream = new CryptoStream(stream, Cipher.CreateDecryptor(), CryptoStreamMode.Read);

            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(decStream);

            decStream.Close();
            stream.Close();

            return obj;
        }


        public static void EncryptTestFile(string plaintext, string path)
        {
            byte[] bytes = Encrypt(plaintext, Cipher);
            File.WriteAllBytes(path, bytes);
        }

        public static string DecriptTestFile(string path)
        {
            byte[] ciphertext = File.ReadAllBytes(path);
            string plaintext = Decript((byte[])ciphertext.Clone(), Cipher);

            return plaintext;
        }

        #endregion
    }
}
