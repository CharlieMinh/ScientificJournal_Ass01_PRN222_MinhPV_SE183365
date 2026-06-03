using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ScientificJournal.Services.MinhPV.Interfaces;
using System.Security.Claims;
using WebApplication1.Models;

namespace ScientificJournal.WebMVCApp.MinhPV.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersHuyDdService _usersService;

        public AccountController(IUsersHuyDdService usersService)
        {
            _usersService = usersService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (await _usersService.EmailExistsAsync(request.Email))
            {
                ModelState.AddModelError(nameof(request.Email), "This email is already registered.");
                return View(request);
            }

            await _usersService.RegisterAsync(request.FullName, request.Email, request.Password);
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var user = await _usersService.LoginAsync(request.Email, request.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Login failure");
                return View(request);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserIdHuyDd.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in user.RoleIdHuyDds)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                claims.Add(new Claim(ClaimTypes.Role, role.RoleIdHuyDd.ToString()));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            Response.Cookies.Append("UserName", user.FullName);
            Response.Cookies.Append("Email", user.Email);

            return RedirectToAction("Index", "Journals");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("Email");
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
