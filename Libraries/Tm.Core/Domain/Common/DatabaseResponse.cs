using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Common
{
    public partial class DatabaseResponse : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the ErrorCode
        /// </summary>
        public string ErrorCode { get; set; }
    }
}
