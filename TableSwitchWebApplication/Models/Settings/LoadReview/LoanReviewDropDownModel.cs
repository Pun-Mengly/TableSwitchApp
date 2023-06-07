using TableSwitchWebApplication.Models.Settings.LoadReview.LoadReview;

namespace TableSwitchWebApplication.Models.Settings.LoadReview
{
    public class LoanReviewDropDownModel
    {
        public ICollection<T24_COForLoanReviewDDLGetModel>? LoanReviewCOModels { get; set; }
        public ICollection<GetLookUpByDevice_EnhModel>? GetLookUpByDevice_EnhModels { get; set; }
        public ICollection<T24_LookUpModel>? T24_LookUpModels { get; set; }
    }
}
