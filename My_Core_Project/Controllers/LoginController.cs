using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using My_Core_Project.Services;
using System.Security.Claims;

namespace My_Core_Project.Controllers
{
    public class LoginController : Controller
    {
        IUser user;

        public LoginController(IUser user)
        {
            this.user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM obj)
        {
            if (ModelState.IsValid)
            {
                var Validuser = user.Login(obj.Username, obj.Password);
                if (Validuser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Validuser.Username),
                        new Claim(ClaimTypes.Role, Validuser.Role),
                        new Claim("IsAdmin", Validuser.IsAdmin?.ToString() ?? "False")
                    };

                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

                    TempData["LoginSuccess"] = "Welcome back, " + Validuser.Username + "!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["LoginError"] = "Invalid Username or Password";
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["LogoutMessage"] = "You have been logged out successfully!";
            return RedirectToAction("Index");
        }
    }
}
