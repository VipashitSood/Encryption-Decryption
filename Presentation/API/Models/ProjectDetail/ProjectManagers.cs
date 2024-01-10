using System;

namespace API.Models.ProjectDetail
{
    public class ProjectManagers
    {
        public int Id { get; set; }
        public string ManagerName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public int ProjectId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int EmpId { get; set; }
    }
    public class ResourceDTO
    {
        public int ProjectId { get; set; }
        public int ForYear { get; set; }
        public int ForMonth { get; set; }
        public int EmployeeId { get; set; }
        public int Role { get; set; }
        public int ResourceType { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int Billable { get; set; }
        public bool Status { get; set; } = true;
    }
}
