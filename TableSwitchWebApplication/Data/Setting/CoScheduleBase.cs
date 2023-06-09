using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Xml.Linq;
using TableSwitchWebApplication.BusineseLogics;
using TableSwitchWebApplication.Models.Settings.Schedule;

namespace TableSwitchWebApplication.Data.Setting
{
    public class CoScheduleBase:ComponentBase
    {
        [Inject]
        protected IBusineseLogic? _logic { set; get; }
        public IEnumerable<UserShecduleModel>? PMShecduleModels { get; set; }
        public IEnumerable<UserShecduleModel>? BMShecduleModels { get; set; }
        public IEnumerable<UserShecduleModel>? AMShecduleModels { get; set; }
        public IEnumerable<UserShecduleModel>? CoShecduleModels { get; set; }
        public UserShecduleModel? CoShecdule { get; set; }
        public HolidayResponse? Holidays { get; set; }
        public List<Appointment>? Appointments { get; set; } = new();


        

        protected async void OnChangePM(object pm)
        {
            UserShecduleModel user = (UserShecduleModel)pm;
            BMShecduleModels = await _logic!.GetBMByRoleAsync(user.NodeId!);
            if(BMShecduleModels.Count()==0) { BMShecduleModels = null; AMShecduleModels = null; CoShecduleModels = null; } 

        }
        protected async void OnChangeBM(object bm)
        {
            UserShecduleModel user = (UserShecduleModel)bm;
            AMShecduleModels = await _logic!.GetAMByRoleAsync(user.NodeId!);
            if (AMShecduleModels.Count() == 0) { AMShecduleModels = null; CoShecduleModels = null; }
        }
        protected async void OnChangeAM(object am)
        {
            UserShecduleModel user = (UserShecduleModel)am;
            CoShecduleModels = await _logic!.GetCOByRoleAsync(user.NodeId!);
            if (CoShecduleModels.Count() == 0) { CoShecduleModels = null; }
        }
        protected void OnChangeCO(object co)
        {
            UserShecduleModel user = (UserShecduleModel)co;
            CoShecdule = user;
        }
        protected async Task OnSearchAsync()
        {
            //Appointments.Clear();
            //var data = await _logic!.GetPublicHolidayAsync();

            //var publicHolidays = data.DataList!.Where(e => e.Description != "").ToList();

            //foreach (var publicHoliday in publicHolidays)
            //{
            //    var apm = new Appointment()
            //    {
            //        PlanDateStart = publicHoliday.HolidayDate,
            //        PlanDateEnd = publicHoliday.HolidayDate,
            //        Title = publicHoliday.Description
            //    };
            //    Appointments!.Add(apm);
            //}

            Appointments= await _logic!.GetHolidayAsync(CoShecdule==null?"": CoShecdule.NodeId!) as List<Appointment>; //Fixed User CO For Test

        }

    }
}
