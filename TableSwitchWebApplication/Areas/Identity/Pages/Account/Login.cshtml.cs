using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core.Tokenizer;
using TableSwitchWebApplication.BusineseLogics;
using TableSwitchWebApplication.Models.UserAd;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using TableSwitchWebApplication.Data;

namespace TableSwitchWebApplication.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusineseLogic _logic;
        private readonly IJSRuntime _jsRuntime;
        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            IBusineseLogic logic,
            IJSRuntime jsRuntime)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logic = logic;
            _jsRuntime = jsRuntime;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public string? ReturnUrl { get; set; }

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }
        public async Task<IActionResult> OnPostAsync()
        {

            ReturnUrl = Url.Content($"~/");
            if(ModelState.IsValid)
            {
                var result=await _signInManager.PasswordSignInAsync(Input.Username,"Amk@123", false,lockoutOnFailure:false);
                SSOModelRequest sso = new SSOModelRequest()
                {
                    userid= "1",
                    api_name= "TabletWeb",
                    msgid="1",
                    password=Input.Password,
                    username=Input.Username,
                };
                var ADUserResult= await _logic.CallADAuthenticationAsync(sso);
                if (ADUserResult[0]== "Succeed")
                {
                    if (result.Succeeded) {
                        return LocalRedirect(ReturnUrl);
                    } 
                }
                else
                {
                    return Redirect("Logout");
                }
            }
            return Page();
        }
        
        public class InputModel
        {
            [Required]
             public string? Username { get; set; }
            [Required]
            [DataType(DataType.Password)] public string? Password { get; set; }
        }
    }
}
