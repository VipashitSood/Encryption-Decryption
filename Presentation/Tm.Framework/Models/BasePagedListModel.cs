﻿using System.Collections.Generic;

namespace Tm.Framework.Models
{
    /// <summary>
    /// Represents the base paged list model (implementation for DataTables grids)
    /// </summary>
    public abstract partial class BasePagedListModel<T> : BaseTmModel, IPagedModel<T> where T : BaseTmModel
    {
        /// <summary>
        /// Gets or sets data records
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets draw
        /// </summary>
        public string Draw { get; set; }

        /// <summary>
        /// Gets or sets a number of filtered data records
        /// </summary>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets a number of total data records
        /// </summary>
        public int RecordsTotal { get; set; }        
    }
}