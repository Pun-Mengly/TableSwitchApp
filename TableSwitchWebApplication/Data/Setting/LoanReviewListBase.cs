using Microsoft.AspNetCore.Components;
using TableSwitchWebApplication.Models.Settings.LoadReview.LoadReview;
using TableSwitchWebApplication.Models.Settings.LoadReview;
using TableSwitchWebApplication.BusineseLogics;
using TableSwitchWebApplication.Helper;

namespace TableSwitchWebApplication.Data.Setting
{
    public partial class LoanReviewListBase : ComponentBase
    {
        [Inject]
        protected IBusineseLogic? _logic { get; set;}
        protected string title = "loan review list";
        protected DateTime? fromdate { get; set; } = DateTime.Now;
        protected DateTime? todate { get; set; } = DateTime.Now;
        protected IEnumerable<T24_COForLoanReviewDDLGetModel>? co;
        protected IEnumerable<LoanReviewModel>? loans;
        protected LoanReviewDropDownModel? allDropdown;
        protected int CoId;
        protected List<string> coText = new();
        protected ICollection<T24_AMLCusGetModel>? T24_AMLCusGetModels { get; set; }
        protected CustomerDetailModel? CustomerDetailModels { get; set; }


        protected void OnChangeCO(object outlet)
        {
            CoId = co!.Where(e=>e.Text== outlet.ToString()).Select(e=>e.Id).FirstOrDefault();
        }
        protected async Task btnSearch()
        {
            try
            {
                loans = await _logic!.GetBtnSearchLoanReviewAsync(userId: GlobalData.UserId, fromdate: fromdate, toDate: todate, CoId: CoId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void AppFormAction(string txt)
        {
            Console.WriteLine(txt);
        }

        protected bool dialogIsOpenNoHit = false;
        protected bool dialogIsOpenCustomerDetail = false;

        protected async void OpenDialogNoHit(string loanAppID)
        {
            dialogIsOpenNoHit = true;
            await GetByLoanAppID(loanAppID);
        }

        protected void DialogIsOpenChangedNoHit(bool isOpen)
        {
            dialogIsOpenNoHit = isOpen;
        }
        protected async void OpenDialogCustomerDetail(string loanAppID,string loanAppPersonID)
        {
            dialogIsOpenCustomerDetail = true;
            await GetCustomerDetail(loanAppID, loanAppPersonID);
        }

        protected void DialogIsOpenChangedCustomerDetail(bool isOpen)
        {
            dialogIsOpenCustomerDetail = isOpen;
        }
        protected async Task GetByLoanAppID(string loanAppID)
        {
            var result= await _logic!.GetT24_AMLCusGetLoanReviewAsync(loanAppID);
            T24_AMLCusGetModels = result.ToList();
        }
        protected async Task GetCustomerDetail(string loanAppID,string loanAppPersonID)
        {
            var result = await _logic!.GetT24_AMLCusGetCustomerDetailAsync(loanAppID, loanAppPersonID);
            CustomerDetailModels = result;
        }

    }
}
