using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API.Models.ProjectDetail
{
    //public class ADProjects
    //{
    //    [JsonProperty("@odata.context")]
    //    public string odatacontext { get; set; }

    //    [JsonProperty("@odata.nextLink")]
    //    public string odatanextLink { get; set; }
    //    public List<Value> value { get; set; }
    //}

    public class ADProjects
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        //public int revision { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

}
