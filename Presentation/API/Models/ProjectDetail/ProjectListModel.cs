namespace API.Models.ProjectDetail
{
    public class ProjectListModel
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
        public string ProjectStatus { get; set;}
        /// <summary>
        /// Get or set the project is on Azure or not
        /// </summary>
        public bool IsAzure { get; set; }

    }
}
