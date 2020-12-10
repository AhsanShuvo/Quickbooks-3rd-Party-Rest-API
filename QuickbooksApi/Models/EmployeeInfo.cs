namespace QuickbooksApi.Models
{
    public class EmployeeInfo
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string SyncToken { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }
}