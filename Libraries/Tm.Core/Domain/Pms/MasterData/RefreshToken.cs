using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.MasterData
{
    public partial class RefreshToken : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public string UserId { get; set; }
        
    }
}
