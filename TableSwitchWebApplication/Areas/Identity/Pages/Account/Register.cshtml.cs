using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TableSwitchWebApplication.Data;

namespace TableSwitchWebApplication.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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
            ReturnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                //string maxUserId=_userManager!.Users!.Max(e => e.UserID)!;
                var maxUserId = _userManager!.Users!.Select(c => Convert.ToInt32(c.UserID)).Max();

                var identity = new ApplicationUser { UserName = Input.UserName, Email = Input.UserName+"@amkcambodia.com",UserID= (maxUserId++).ToString()};
                var result = await _userManager.CreateAsync(identity, "Amk@123");
                var role = new IdentityRole(Input.Role);
                var addRoleResult = await _roleManager.CreateAsync(role);
                var addUserRoleResult=await _userManager.AddToRoleAsync(identity,Input.Role);
                if (result.Succeeded && addUserRoleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(identity, isPersistent: false);
                    return Redirect("Login");
                }
            }
            return Page();
        }
        public class InputModel
        {
            [Required]
            public string? UserName { get; set; }
            //[Required]
            //[DataType(DataType.Password)] public string? Password { get; set; }
            [Required]
            public string? Role { get; set; } 
        }
    }
}
