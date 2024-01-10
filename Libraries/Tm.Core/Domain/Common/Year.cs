using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tm.Core.Domain.Common
{
    public enum Year
    {
        [Display(Name = "January")]
        Jan = 1,
        [Display(Name = "February")]
        Feb = 2,
        [Display(Name = "March")]
        Mar = 3,
        [Display(Name = "April")]
        Apr = 4,
        [Display(Name = "May")]
        May = 5,
        [Display(Name = "June")]
        Jun = 6,
        [Display(Name = "July")]
        Jul = 7,
        [Display(Name = "August")]
        Aug = 8,
        [Display(Name = "September")]
        Sep = 9,
        [Display(Name = "October")]
        Oct = 10,
        [Display(Name = "November")]
        Nov = 11,
        [Display(Name = "December")]
        Dec = 12
    }
}
