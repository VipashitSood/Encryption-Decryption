using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class Extensions
    {
        public static decimal ToRound(this decimal? d)
        {
            if (d == null)
                return 0;
            return Math.Round((decimal)d, 2);
        }
        public static string ToCurrency(this string s)
        {
            return "£" + s; // will refactor this later
        }

        public static string ToCurrencyWithComma(this string s)
        {
            return "£" + (string.IsNullOrEmpty(s)?s:(Convert.ToDecimal(s).ToString("#,##0.00"))); // will refactor this later
        }

        public static string ToRound(this decimal d)
        {
            return Math.Round((decimal)d, 2).ToString();
        }
        public static decimal ToRoundDecimal(this decimal d)
        {
            return Math.Round((decimal)d, 2);
        }

        public static decimal ToRoundDecimalOnePlace(this decimal d)
        {
            return Math.Round((decimal)d, 1);
        }

        public static decimal ToRoundWholeNumber(this decimal d)
        {
            return Math.Floor((decimal)d);
        }

        public static string GetDisplayName(Enum enumValue)
        {
            string displayName;
            displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();
            if (String.IsNullOrEmpty(displayName))
            {
                displayName = enumValue.ToString();
            }
            return displayName;
        }
    }
}
