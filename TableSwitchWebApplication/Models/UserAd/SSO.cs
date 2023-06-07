namespace TableSwitchWebApplication.Models.UserAd
{
    public class SSOModelRequest
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? msgid { get; set; }
        public string? userid { get; set; }
        public string? api_name { get; set; }
    }

    public class SSOModel
    {

        public string? status { get; set; }
        public string? message { get; set; }
        public string? token { get; set; }

    }
}
