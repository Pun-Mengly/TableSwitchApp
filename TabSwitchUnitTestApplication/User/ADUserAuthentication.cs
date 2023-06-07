using TableSwitchWebApplication.BusineseLogics;
using TableSwitchWebApplication.Models.UserAd;

namespace TabSwitchUnitTestApplication.ADUserAuthentication
{
    public class User
    {
        [Test]
        public async Task Test_CallADAuthentication()
        {
            try
            {
                BusineseLogic _logic = new();
                SSOModelRequest sso = new SSOModelRequest()
                {
                    userid = "1",
                    api_name = "TabletWeb",
                    msgid = "1",
                    password = "AMk@22099",
                    username = "CBS.TEST",
                };
                var response=await _logic.CallADAuthenticationAsync(sso);
                var status = response[0];
                Assert.That(status, Is.EqualTo("Succeed"), "User Is Inavalid...");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
