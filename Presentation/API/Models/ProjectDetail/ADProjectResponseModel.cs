using System;

namespace API.Models.ProjectDetail
{
    public class ADProjectResponseModel
    {
            public string id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string state { get; set; }
            public string revision { get; set; }
            public string visibility { get; set; }
            public DateTime lastUpdateTime { get; set; }
    }
}
