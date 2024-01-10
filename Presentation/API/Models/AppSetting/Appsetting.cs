using Microsoft.Extensions.Options;

namespace API.Models.AppSetting
{
    public class JWT
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
    public class AD
    {
        public string ClientId { get; set; }
        public string Scope { get; set; }
        public string RedirectUri { get; set; }
        public string GrantType { get; set; }
        public string ClientSecret { get; set; }
        public string EndPointURL { get; set; }
        public string AllUserEndPoint { get; set; }
        public string Count { get; set; }
        public string StsDiscoveryEndpoint { get; set; }
        public string ByPassAuthMethod { get; set; }
        public string AllProjects { get; set; }
        public string ProjectsByName { get; set; }
        public string PersonalAccessToken { get; set; }
        public string TimeLogAPIUrl { get; set; }
        public string OrganisationId { get; set; }
        public string TimeLogClientCode { get; set; }
        public int TimeLogsFromPreviousDays { get; set; }
    }
}
