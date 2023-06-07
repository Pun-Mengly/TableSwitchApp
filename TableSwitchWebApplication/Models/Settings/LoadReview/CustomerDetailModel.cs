namespace TableSwitchWebApplication.Models.Settings.LoadReview
{
    public class CustomerDetailModel
    {
        public ICollection<T24_AMLDetailGetModel>? t24_AMLDetailGet { get; set; }
        public ICollection<T24_AMLCusKYCGetModel>? t24_AMLCusKYCGet { get; set; }
        public ICollection<Person_img_listModel>? person_Img_List { get; set; }
    }
}
