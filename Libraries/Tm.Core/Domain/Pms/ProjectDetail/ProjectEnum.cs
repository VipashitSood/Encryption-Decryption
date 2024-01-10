namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public enum ProjectEnum
    {
        /// <summary>
        /// Other
        /// </summary>
        Other = -1,
        /// <summary>
        /// BackendTechStack
        /// </summary>
        BackendTechStack = 1,
        /// <summary>
        /// FrontendTechStack
        /// </summary>
        FrontendTechStack = 2
    }
    public enum Months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    public enum ResourceType
    {
        Core,
        Shadow,
        Support,
        Learner
    }
    public enum Role
    {
        Backend,
        Frontend,
        QA,
        BA
    }
}
