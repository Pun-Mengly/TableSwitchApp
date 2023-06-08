namespace TableSwitchWebApplication.Models.Settings.Schedule
{
    public class Appointment
    {
        public string? TaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? PlanDateStart { get; set; }
        public DateTime? PlanDateEnd { get; set; }
        public DateTime? ActualDateStart { get; set; }
        public DateTime? ActualDateEnd { get; set; }
        public string? OwnerUserId { get; set; }
        public string? OwnerUserText { get; set; }
        public string? TaskTypeId { get; set; }
        public string? TaskTypeStatusId { get; set; }
        public string? Remark { get; set; }
        public string? CreatedDate { get; set; }
    }

    public class COScheduleResponse
    {
        public string? ERR { get; set; }
        public string? SMS { get; set; }
        public string? ERRCode { get; set; }
        public List<Appointment>? DataList { get; set; }
    }
   

}
