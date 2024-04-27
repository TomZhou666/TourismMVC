using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourismMVC.Data;

namespace TourismMVC.Controllers
{
    public class RolesController : Controller
    {
        private string AdminRole = "Admin";
        private string AdminEmail = "Admin@outlook.com";
        private string password = "Tom@@123";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RolesController(ApplicationDbContext _context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            this._context = _context;
            this.userManager = userManager;
            this.roleManager = roleManager;
         }
        public async Task<IActionResult> Index()
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
            IdentityUser admin = new IdentityUser { UserName = AdminEmail, Email = AdminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(admin, password);
            await userManager.AddToRoleAsync(admin, AdminRole);
            return Redirect("/");
        }
    }
}
