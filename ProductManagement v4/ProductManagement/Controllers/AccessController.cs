using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ProductManagement.Models;
using ProductManagement.BLL.Services.EmployeeService;

namespace ProductManagement.Controllers
{
    public class AccessController : Controller
    {
        private readonly IEmployeeService employeeService;
        public AccessController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccessViewModel accessModel)
        {
            if (ModelState.IsValid)
            {
                var authentication = employeeService.AuthenticateEmployee(accessModel.Email, accessModel.Password);
                if (authentication == null)
                {
                    ViewData["ValidateMessage"] = "Incorrect email or password.";
                    return View();
                }

                List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, authentication.Email),
                new Claim(ClaimTypes.Email, authentication.Email),
                new Claim(ClaimTypes.Role, authentication.Role),
                new Claim(ClaimTypes.Name, authentication.FirstName),
                new Claim(ClaimTypes.Surname, authentication.LastName)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = accessModel.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            return View();

        }
    }
}
