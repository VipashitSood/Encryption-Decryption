using System;

namespace API.Models.Attendance
{
    public class ADTimeLog
    {
        public string TimeLogId { get; set; }
        public string Comment { get; set; }
        public string Week { get; set; }
        public string TimeTypeId { get; set; }
        public string TimeTypeDescription { get; set; }
        public int Minutes { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ProjectId { get; set; }
        public int WorkItemId { get; set; }

    }
}
