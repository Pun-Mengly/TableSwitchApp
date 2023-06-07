using TableSwitchWebApplication.Data;
using TableSwitchWebApplication.Models;
using TableSwitchWebApplication.Models.Settings.LoadReview;
using TableSwitchWebApplication.Models.UserAd;

namespace TableSwitchWebApplication.BusineseLogics
{
    public interface IBusineseLogic
    {
        public Task<IEnumerable<OfficeModel>> GetOfficePaginationAsync(int PageNumber,int PageSize);
        public Task<IEnumerable<UserModel>> GetAllUserAsync();
        public Task<LoanReviewDropDownModel> GetAllDropDownLoanReviewAsync(int userId);
        public Task<IEnumerable<LoanReviewModel>> GetBtnSearchLoanReviewAsync(string? userId,DateTime? fromdate, DateTime? toDate, int CoId);
        public Task<IEnumerable<T24_AMLCusGetModel>> GetT24_AMLCusGetLoanReviewAsync(string? loanAppID);
        public Task<CustomerDetailModel> GetT24_AMLCusGetCustomerDetailAsync(string? loanAppID, string? loanAppPersonID);
        public Task<IEnumerable<ADUserModel>> GetADUsersAsync();
        public Task<string[]> CallADAuthenticationAsync(SSOModelRequest ssoRequest);
        public Task<ApplicationUser> GetCurrentUserAsync();

    }
}
