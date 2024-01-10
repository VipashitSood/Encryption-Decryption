using API.Models.BaseModels;
using System.Collections.Generic;

namespace API.Models.POInformation
{
	public class PoInfoFilterModel :  BaseRequestModel
    {
        public PoInfoFilterModel()
        {
            PoNmmbers = new List<ListItem>();
            Client = new List<ListItem>();
            CompanyName = new List<ListItem>();
        }


        public List<ListItem> PoNmmbers { get; set; }
        public List<ListItem> Client { get; set; }
        public List<ListItem> CompanyName { get; set; }

        public class ListItem
        {
            /// <summary>
            /// Gets or sets the Value
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Gets or sets the Label
            /// </summary>
            public string Label { get; set; }
        }
    }
}
