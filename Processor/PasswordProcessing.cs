using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoginRegistration.Processor
{
    public class PasswordProcessing
    {
        public readonly IConfiguration _configuration;

        /*public PasswordProcessing()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build(); ;
        }*/

        /// <summary>
        /// Declaration of encrypt method
        /// </summary>
        /// <param name="originalString">Passing password string</param>
        /// <returns>return string</returns>
        public static string Encrypt(string originalString, string key)
        {
            //string Key = _configuration["SecurityKey"];
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);

            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes),
                CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Declaration of decrypt method
        /// </summary>
        /// <param name="encryptedString">passing string</param>
        /// <returns>return string</returns>
        public static string Decrypt(string encryptedString, string key)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            if (String.IsNullOrEmpty(encryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedString));
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes),
                CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

    }
}
