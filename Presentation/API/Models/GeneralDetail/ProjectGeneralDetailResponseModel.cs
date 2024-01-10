
namespace API.Models.GeneralDetail
{
    public class ProjectGeneralDetailResponseModel
    {
        public int ProjectId { get; set; }
        public int ProjectNameId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectType { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string ClientName { get; set; }
        public int ProjectStatusId { get; set; }
        public string ProjectStatus { get; set; }
        public string OrderDetailResponseModel { get; set; }
    }
}
