
namespace Tm.Core.Domain.Pms.Attendance
{
    public class AttendenceUserList : BaseEntity
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email{ get; set; }
        public string Mobile{ get; set; }
        public string JobTitle { get; set; }

    }
}
