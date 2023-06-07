using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using RestSharp;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.Json;
using TableSwitchWebApplication.Cores;
using TableSwitchWebApplication.Data;
using TableSwitchWebApplication.Helper;
using TableSwitchWebApplication.Models;
using TableSwitchWebApplication.Models.Settings.LoadReview;
using TableSwitchWebApplication.Models.Settings.LoadReview.LoadReview;
using TableSwitchWebApplication.Models.UserAd;

namespace TableSwitchWebApplication.BusineseLogics
{
    public class BusineseLogic : IBusineseLogic
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NavigationManager _navigationManager;
        public BusineseLogic(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            NavigationManager navigationManager)
        {

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _navigationManager= navigationManager;

        }
        public BusineseLogic()
        {

        }
        #region Loan Review List Logical
        public async Task<LoanReviewDropDownModel> GetAllDropDownLoanReviewAsync(int userId)
        {
            try
            {
                var cos=new List<T24_COForLoanReviewDDLGetModel>();
                string sql = "EXEC T24_GetLookUpByDevice_Enh" + " ";
                sql += "EXEC T24_GetLookUpByDevice @UserID='" + userId + "',@criteriaValue='DisAgreeReason'";
                sql += "SELECT [LOOKUPID],[DESCRIPTION_KH] FROM [T24_LookUp]  where criteriaValue = 'RejectReason' and LOOKUPID in ('R1','R2','R3','R4','R5','R6','R7','R8')";
                sql += "EXEC T24_COForLoanReviewDDLGet @UserID='" + userId + "'";
                var ds = await AdoOperation.ReturnDT2_DATASET(sql);
                var dt_0 = ds.Tables[0];
          
                var dt_1 = ds.Tables[1];
                var dt_2 = ds.Tables[2];
                var dt_3 = ds.Tables[3];
                foreach (DataRow row in dt_3.Rows)
                {
                    T24_COForLoanReviewDDLGetModel co = new T24_COForLoanReviewDDLGetModel()
                    {
                        Id = Convert.ToInt32(row["ID"]),
                        Text = row["Txt"].ToString(),
                    };
                    cos.Add(co);
                }
                var data = new LoanReviewDropDownModel()
                {
                    T24_LookUpModels = new List<T24_LookUpModel>(),
                    GetLookUpByDevice_EnhModels = new List<GetLookUpByDevice_EnhModel>(),
                    LoanReviewCOModels = cos
                };
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IEnumerable<LoanReviewModel>> GetBtnSearchLoanReviewAsync(string? userId, DateTime? fromdate, DateTime? toDate, int CoId)
        {
            try
            {
                var loans = new List<LoanReviewModel>();
                string sql = $"exec T24_LoanReviewListGet_V2 @UserID='{userId}',@DateFrom='{fromdate}',@DateTo='{toDate}',@COUserID='{CoId}'";
                //string sql = "exec T24_LoanReviewListGet_V2 @UserID='1',@DateFrom='01-Jan-22 12:00:00 AM',@DateTo='23-May-23 9:54:55 AM',@COUserID='245'";
                DataTable dt = new DataTable();
                dt = await AdoOperation.ReturnDT1(sql);
                foreach (DataRow row in dt.Rows)
                {
                    LoanReviewModel loan = new LoanReviewModel()
                    {
                        Id = row["LoanAppId"].ToString(),
                        BranchName= row["LoanAppId"].ToString(),
                        COName= row["COName"].ToString(),
                        LoanAMKDL= row["LoanAMKDL"].ToString(),
                        LoanProduct= row["LoanProduct"].ToString(),
                        LoanCurrency= row["LoanCurrency"].ToString(),
                        LoanAmount= row["LoanAmount"].ToString(),
                        CustomerId= row["CustomerID"].ToString(),
                        FullNameKH= row["FullNameKH"].ToString(),
                        EasySavingAccount= row["SavingACC"].ToString(),
                        CBCReferenceId= row["CBCReferenceID"].ToString(),
                        DeskCheck = row["Desk_Check"].ToString(),
                        PreCheck = row["Pre_Check"].ToString(),
                        Approve= row["Approve"].ToString(),
                        AppForm= row["AppForm"].ToString(),
                        CustomerGrand= row["ScoreGrade"].ToString(),

                    };
                    loans.Add(loan);
                }
                return loans;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public async Task<IEnumerable<T24_AMLCusGetModel>> GetT24_AMLCusGetLoanReviewAsync(string? loanAppID)
        {
            try
            {
                var result=new List<T24_AMLCusGetModel>();
                string query = $"exec T24_AMLCusGet @LoanAppID='{loanAppID}'";
                var dataSource = await AdoOperation.ReturnDT1(query);
                foreach (DataRow row in dataSource.Rows)
                {
                    string? dateString = row["DateOfBirth"].ToString();
                    DateTime date = DateTime.ParseExact(dateString!, "dd-MMM-yy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    string formattedDate = date.ToString("dd-MMM-yyyy");

                    var objT24_AMLCusGet = new T24_AMLCusGetModel()
                    {
                        LoanAppID = row["LoanAppID"].ToString(),
                        CustomerID = row["CustomerID"].ToString(),
                        DateOfBirth = formattedDate,
                        Description = row["Description"].ToString(),
                        FullNameEn = row["FullNameEn"].ToString(),
                        FullNameKh = row["FullNameKh"].ToString(),
                        Gender = row["Gender"].ToString(),
                        IDNumber = row["IDNumber"].ToString(),
                        LoanAppPersonID = row["LoanAppPersonID"].ToString(),
                        LoanAppPersonType = row["LoanAppPersonType"].ToString(),
                        MaritalStatusID  = row["MaritalStatusID"].ToString(),
                        RiskProfiling = row["RiskProfiling"].ToString(),
                        TelePhone3 = row["TelePhone3"].ToString(),
                        VBName = row["VBName"].ToString(),
                        WatchListScreeningStatus = row["WatchListScreeningStatus"].ToString(),
                        BlockStatus = row["BlockStatus"].ToString(),
                    };
                    result.Add(objT24_AMLCusGet);
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CustomerDetailModel> GetT24_AMLCusGetCustomerDetailAsync(string? loanAppID,string? loanAppPersonID)
        {
            try
            {
                var customerDetail=new CustomerDetailModel();
                var t24_AMLDetailGets=new List<T24_AMLDetailGetModel>();
                var t24_AMLCusKYCGets= new List<T24_AMLCusKYCGetModel>();
                var person_img_lists = new List<Person_img_listModel>();
                string sql = "exec T24_AMLDetailGet @LoanAppPersonID='" + loanAppPersonID + "'"; //1st query
                sql += "exec T24_AMLCusKYCGet @LoanAppID='" + loanAppID + "',@LoanAppPersonID='" + loanAppPersonID + "'"; //2nd query
                sql += "exec person_img_list '" + loanAppID + "','" + loanAppPersonID + "'"; //3th query

                var ds = await AdoOperation.ReturnDT2_DATASET(sql);
                #region T24_AMLDetailGet
                var dt = ds.Tables[0];
                foreach (DataRow item in dt.Rows)
                {
                    string? dateString = item["DateOfBirth"].ToString();
                    DateTime date = DateTime.ParseExact(dateString!, "dd-MMM-yy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    string formattedDate = date.ToString("dd-MMM-yyyy");
                    var t24_AMLDetailGet = new T24_AMLDetailGetModel()
                    {
                        BlockStatus = item["BlockStatus"]==null? "":item["BlockStatus"].ToString(),
                        CustomerID = item["CustomerID"] == null ? "" : item["CustomerID"].ToString(),
                        DateOfBirth = item["DateOfBirth"] == null ? "" : formattedDate,
                        faceDocumentScore = item["faceDocumentScore"] == null ? "" : item["faceDocumentScore"].ToString(),
                        faceMoiScore = item["faceMoiScore"] == null ? "" : item["faceMoiScore"].ToString(),
                        FullNameEn = item["FullNameEn"] == null ? "" : item["FullNameEn"].ToString(),
                        FullNameKh = item["FullNameKh"] == null ? "" : item["FullNameKh"].ToString(),
                        Gender = item["Gender"] == null ? "" : item["Gender"].ToString(),
                        IDNumber = item["IDNumber"] == null ? "" : item["IDNumber"].ToString(),
                        isCheck =item["isCheck"] == null ? "" : item["isCheck"].ToString(),
                        jsonDifference = item["jsonDifference"] == null ? "" : item["jsonDifference"].ToString(),
                        LoanAppID = item["LoanAppID"] == null ? "" : item["LoanAppID"].ToString(),
                        LoanAppPersonID= item["LoanAppPersonID"] == null ? "" : item["LoanAppPersonID"].ToString(),
                        Nationality = item["Nationality"] == null ? "" : item["Nationality"].ToString(),
                        OccupationDetail = item["OccupationDetail"] == null ? "" : item["OccupationDetail"].ToString(),
                        OccupationType = item["OccupationType"] == null ? "" : item["OccupationType"].ToString(),
                        RiskProfiling = item["RiskProfiling"] == null ? "" : item["RiskProfiling"].ToString(),
                        StatusRS = item["StatusRS"] == null ? "" : item["StatusRS"].ToString(),
                        Telephone3 = item["Telephone3"] == null ? "" : item["Telephone3"].ToString(),
                        Title = item["Title"] == null ? "" : item["Title"].ToString(),
                        UserScore = item["UserScore"] == null ? "" : item["UserScore"].ToString(),
                        WatchListCaseUrl = item["WatchListCaseUrl"] == null ? "" : item["WatchListCaseUrl"].ToString(),
                        WatchListExposition = item["WatchListExposition"] == null ? "" : item["WatchListExposition"].ToString(),

                    };
                    t24_AMLDetailGets.Add(t24_AMLDetailGet);
                }
                #endregion
                #region T24_AMLCusKYCGet
                var dt1 = ds.Tables[1];
                foreach (DataRow row in dt1.Rows)
                {
                    var t24_AMLDetailGet = new T24_AMLCusKYCGetModel()
                    {
                        IDExpireDate = row["IDExpireDate"].ToString(),
                        IDIssueDate = row["IDIssueDate"].ToString(),
                        IDNumber = row["IDNumber"].ToString(),
                        IDTYPE = row["IDTYPE"].ToString(),
                        isCurrentKYC = row["isCurrentKYC"].ToString(),
                        
                    };
                    t24_AMLCusKYCGets.Add(t24_AMLDetailGet);
                }
                #endregion
                #region Person_img_list
                var dt2 = ds.Tables[2];

                foreach (DataRow row in dt2.Rows)
                {
                    var person_img_list = new Person_img_listModel()
                    {
                        DocType= row["DocType"].ToString(),
                        ext= row["ext"].ToString(),
                        ImgLogID= row["ImgLogID"].ToString(),
                        IsChange= row["IsChange"].ToString(),
                        IsFaceImg= row["IsFaceImg"].ToString(),
                        LoanAppID= row["LoanAppID"].ToString(),
                        ServerDate= row["ServerDate"].ToString(),
                        ServerImgName= row["ServerImgName"].ToString(),
                        tblKey= row["tblKey"].ToString(),
                        tblName= row["tblName"].ToString(),
                        
                    };
                    person_img_lists.Add(person_img_list);
                }
                #endregion
                #region Customer Detail
                customerDetail = new CustomerDetailModel()
                {
                    person_Img_List=person_img_lists,
                    t24_AMLDetailGet= t24_AMLDetailGets,
                    t24_AMLCusKYCGet = t24_AMLCusKYCGets,
                };
                #endregion
                return customerDetail;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region User Logical
        public async Task<IEnumerable<UserModel>> GetAllUserAsync()
        {
            List<UserModel> users = new List<UserModel>();
            var dataTable = await AdoOperation.ReturnDT1("select *from tblUserDec where ADUser is not null");

            foreach (DataRow row in dataTable.Rows)
            {
                UserModel user = new UserModel()
                {
                    Id = Convert.ToInt32(row["UserId"]),
                    Username = row["UserName"].ToString(),
                    UserAd = row["ADUser"].ToString(),
                    Deleted = Convert.ToBoolean(row["IsDelete"]),
                };
                users.Add(user);
            }
            return users;
        }
        #endregion

        #region Office Logical
        public async Task<IEnumerable<OfficeModel>> GetOfficePaginationAsync(int PageNumber, int PageSize)
        {
            using (SqlConnection connection = new SqlConnection(AppCon.ConStr))
            {
                var result = new List<OfficeModel>();
                await connection.OpenAsync();
                // your code here
                using (SqlCommand command = new SqlCommand("GetOfficeByPage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@PageSize", PageSize);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new OfficeModel()
                            {
                                Id = reader.GetString(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("txt")),
                                TotalPage = reader.GetInt32(reader.GetOrdinal("TotalPage"))
                            };
                            result.Add(row);
                        }
                    }
                }
                await connection.CloseAsync();
                return result;
            }
        }


        #endregion

        #region Login With ADUser
       
        public async Task<IEnumerable<ADUserModel>> GetADUsersAsync() // all user entry
        {
            try
            {
                var result = new List<ADUserModel>();
                string sql = $"EXEC LoginWithADUsers";
                var dt = await AdoOperation.ReturnDT1(sql);
                foreach (DataRow row in dt.Rows)
                {
                    var loginADUser = new ADUserModel()
                    {
                        UserID = row["UserID"].ToString(),
                        ADUser = row["ADUser"].ToString(),
                        Email = row["Email"].ToString(),
                        GroupName = row["GroupName"].ToString(),
                    };
                    result.Add(loginADUser);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string[]> CallADAuthenticationAsync(SSOModelRequest ssoRequest)
        {
            string status = "", message = "", token = "";

            try
            {

                string url = AppCon.ADUserConStr + "/api/LoginAD";
                var client = new RestClient(url);
                //client.Timeout = -1;
                RestRequest request = new RestRequest("",Method.Post);
                request.AddHeader("Authorization", "Basic YW1rOjEyMw==");
                request.AddHeader("Content-Type", "application/json");
                var body = JsonSerializer.Serialize(ssoRequest);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                string strRS = response.Content!.ToString();
                string mystr = strRS.Substring(1, strRS.Length - 2);

                SSOModel ssoObj = JsonSerializer.Deserialize<SSOModel>(mystr)!;
                status = ssoObj.status!;
                message = ssoObj.message!;
                token = ssoObj.token!;

            }
            catch (Exception ex)
            {
                status = "Error";
                message = "(00012) Invalid user/criteria " + ex.Message.ToString();
                token = "";
            }

            string[] rs = { status, message, token };
            return rs;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            //var name= _httpContextAccessor.HttpContext!.User.Identity!.Name;
            var name = GlobalData.UserName;

            if(name is null )
            {
                _navigationManager.NavigateTo("Identity/Account/Login");
            }
            var user = await _userManager.FindByNameAsync(name);
            return user;
        }
        #endregion

        #region Authentication HangFire Filter
        public async Task<IEnumerable<AuthHangFireFilterModel>> GetAuthHangFireAsync(string? UserAD) // Specific user entry for Hang Fire Authentication
        {
            try
            {
                var result = new List<AuthHangFireFilterModel>();
                string sql = $"EXEC AuthHangFireFilter @UserAD = '{UserAD}'";
                var dt = await AdoOperation.ReturnDT1(sql);
                foreach (DataRow row in dt.Rows)
                {
                    var loginADUser = new AuthHangFireFilterModel()
                    {
                        UserID = row["UserID"].ToString(),
                        UserName = row["UserName"].ToString(),
                        Id = row["Id"].ToString(),
                        Role = row["Role"].ToString(),
                    };
                    result.Add(loginADUser);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}

