using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tm.Services.Pms.EncryptDecrypt
{
    public interface IEncryptDecryptService
    {
        /// <summary>
        /// Encrypt the order cost
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       string EncryptString(decimal input);

        /// <summary>
        /// Decrypt the order cost
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       decimal DecryptString(string input);
    }
}
