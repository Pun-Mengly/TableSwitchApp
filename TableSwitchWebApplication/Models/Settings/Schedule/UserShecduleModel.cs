namespace TableSwitchWebApplication.Models.Settings.Schedule
{
    public class UserShecduleModel
    {
        public string? NodeId { get; set; }
        public string? ParentId { get; set; }
        public string? Position { get; set; }
        public string? NodeText { get; set; }
        public string? GroupId { get; set; }
        public override string ToString()
        {
            return $"{NodeText}";
        }
    }
}
