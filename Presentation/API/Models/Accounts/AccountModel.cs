using System;
using Tm.Framework.Mvc.ModelBinding;
using Tm.Framework.Models;

namespace API.Models.Accounts
{
    public partial class AccountModel : BaseTmEntityModel
    {
        public string AccountNo { get; set; }

        public string AccountHolderName { get; set; }

    }
}