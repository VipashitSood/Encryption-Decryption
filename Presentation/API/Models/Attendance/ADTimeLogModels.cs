using System;

namespace API.Models.Attendance
{
    public class ADTimeLogModels
    {
        public string timeLogId { get; set; }
        public string comment { get; set; }
        public string week { get; set; }
        public string timeTypeId { get; set; }
        public string timeTypeDescription { get; set; }
        public int minutes { get; set; }
        public DateTime date { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string projectId { get; set; }
        public int workItemId { get; set; }

    }
}
