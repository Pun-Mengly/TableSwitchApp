namespace TableSwitchWebApplication.Models.Settings.LoadReview
{
    public class GetLookUpByDevice_EnhModel
    {
        public int Id { get; set; }
        public string? CriteriaValue { get; set; }
        public string? Description { get; set; }
        public string? ParentId { get; set;}
        public string? OrderBy { get; set;}
        public string? parentCriteriaValue { get; set;}
    }
}
