using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TableSwitchWebApplication.Data
{
    public class DataContext:IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions options):base(options)
        {
            
        }
    }
    public class ApplicationUser : IdentityUser
    {
        public string? UserID { get; set; }
    }
}
