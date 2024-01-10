using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tm.Core.Configuration;
using Tm.Core.Domain.Customers;

namespace API.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// Validate public key
        /// </summary>
        /// <param name="tmConfig"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ValidatePublicKey(TmConfig tmConfig, string key)
        {
            return (key == tmConfig.PublicKey);
        }

        /// <summary>
        /// Generate jwt tokn
        /// </summary>
        /// <param name="tmConfig"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(TmConfig tmConfig, Customer customer)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tmConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", customer.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Get month number by its name
        /// </summary>
        /// <param name="monthName"></param>
        /// <returns></returns>
        public static int GetMonthNumber(string monthName)
        {
            monthName = monthName.ToLower();
            var monthIndex = 1;
            for (int m = 1; m <= 12; m++)
            {
                var dt = new DateTime(DateTime.Now.Year, m, 1);
                if (dt.ToString("MMMM").ToLower() == monthName || dt.ToString("MMM").ToLower() == monthName)
                    monthIndex = m;
            }
            return monthIndex;
        }
    }
}
