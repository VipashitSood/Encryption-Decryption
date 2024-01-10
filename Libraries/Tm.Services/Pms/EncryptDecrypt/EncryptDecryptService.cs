using Microsoft.Extensions.Configuration;
using Renci.SshNet.Security;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tm.Core.Constants;

namespace Tm.Services.Pms.EncryptDecrypt
{
    public partial class EncryptDecryptService : IEncryptDecryptService
    {
        #region  Fields
        private readonly IConfiguration _configuration;
        #endregion

        #region ctor
        public EncryptDecryptService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        /// <summary>
        /// Encrypt the order cost
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string EncryptString(decimal input)
        {
            var encrypDecryptSettings = _configuration.GetSection("EncryptionSettings");
            var key = string.Empty;
            var iv = string.Empty;

            if (encrypDecryptSettings != null && !string.IsNullOrEmpty(encrypDecryptSettings["Key"]))
                key = encrypDecryptSettings["Key"];

            if (encrypDecryptSettings != null && !string.IsNullOrEmpty(encrypDecryptSettings["IV"]))
                iv = encrypDecryptSettings["IV"];

            string inputString = input.ToString();
            if (string.IsNullOrEmpty(inputString))
            {
                return null;
            }
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] ciphertext;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plaintextBytes = Encoding.UTF8.GetBytes(inputString);
                    ciphertext = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
                }

                return Convert.ToBase64String(ciphertext);
            }

        }

        /// <summary>
        /// Decrypt the order cost
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public decimal DecryptString(string input)
        {
            var encrypDecryptSettings = _configuration.GetSection("EncryptionSettings");
            var key = string.Empty;
            var iv = string.Empty;

            if (encrypDecryptSettings != null && !string.IsNullOrEmpty(encrypDecryptSettings["Key"]))
                key = encrypDecryptSettings["Key"];

            if (encrypDecryptSettings != null && !string.IsNullOrEmpty(encrypDecryptSettings["IV"]))
                iv = encrypDecryptSettings["IV"];

            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] ciphertext = Convert.FromBase64String(input);
            string decryptedText;

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                decryptedText = Encoding.UTF8.GetString(decryptedBytes);
            }

            if (decimal.TryParse(decryptedText, out decimal result))
            {
                return result;
            }

            return 0;
        }

    }
}
