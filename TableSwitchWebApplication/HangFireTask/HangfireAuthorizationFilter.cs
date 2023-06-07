using Hangfire.Dashboard;
using TableSwitchWebApplication.BusineseLogics;
using TableSwitchWebApplication.Helper;

namespace TableSwitchWebApplication.HangFireTask
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _roles;

        public HangfireAuthorizationFilter(string roles)
        {
            _roles = roles;
        }

        public bool Authorize(DashboardContext context)
        {
            //Your authorization logic goes here.
            var _logic=new BusineseLogic();
            var user = _logic.GetAuthHangFireAsync(GlobalData.UserName);
            user.Wait();
            if(user.IsCompletedSuccessfully == true)
            {
                foreach (var usr in user.Result)
                {
                    if (usr.Role is null) return false;
                    if (usr.Role == _roles) return true; //Allow only Admin user
                }
                
            }
            return false; //Not Allow Normal users
        }
    }
}
