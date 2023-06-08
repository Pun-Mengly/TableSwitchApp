namespace TableSwitchWebApplication.Models.Settings.Schedule
{
    public class HolidayResponse
    {
        public string? ERR { get; set; }
        public string? SMS { get; set; }
        public string? ERRCode { get; set; }
        public List<PublicHolidayModel>? DataList { get; set; }
    }
    public class PublicHolidayModel
    {
        public string? OrderNo { get; set; }
        public string? HolidayID { get; set; }
        public DateTime? HolidayDate { get; set; }
        public string? Description { get; set; }
    }
    
}
