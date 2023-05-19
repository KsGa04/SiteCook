using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteCook.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SiteCook.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        [AllowAnonymous]
        public IActionResult Authorization()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authorization(string loginOrEmail, string password, string role)
        {
            if (role == "Админ") {
                using (CookingBookContext db = new CookingBookContext())
                {
                    Administrator? administrator = (from o in db.Administrators where o.Mail == loginOrEmail && o.Password == password select o).FirstOrDefault();
                    if (administrator is not null)
                    {
                        return RedirectToAction("Stat");
                    }
                    else
                    {
                        ViewBag.Enter = "Неверные данные для входа";
                        return View();
                    }
                }
            }
            else if (role == "Модератор")
            {
                using (CookingBookContext db = new CookingBookContext())
                {
                    Moderator? moderator = (from o in db.Moderators where o.Mail == loginOrEmail && o.Password == password select o).FirstOrDefault();
                    if (moderator is not null)
                    {
                        return RedirectToAction("Stat");
                    }
                    else
                    {
                        ViewBag.Enter = "Неверные данные для входа";
                        return View();
                    }
                }
            }
            else if (role =="Пользователь")
            {
                using (CookingBookContext db = new CookingBookContext())
                {
                    CurrentUser.CurrentClientId = (from c in db.Users where (c.Mail == loginOrEmail) && c.Password == password select c.IdUser).FirstOrDefault();
                    User client = (from c in db.Users where (c.Mail == loginOrEmail) && c.Password == password select c).FirstOrDefault();
                    if (CurrentUser.CurrentClientId > 0)
                    {
                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "test"),
                        new Claim(ClaimTypes.Email, "testc@mail.ru")};
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return View("Main");
                    }
                    else
                    {
                        ViewBag.Enter = "Неверные данные для входа";
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Enter = "Выберите роль";
                return View();
            }

        }
        [AllowAnonymous]
        public IActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Registration(string email, string password)
        {
            using (CookingBookContext db = new CookingBookContext())
            {
                int countClients = (from c in db.Users where c.Mail == email select c).Count();
                if (countClients > 0)
                {
                    ViewBag.Enter = "Пользователь с данным логином или email уже зарегистрирован";
                    return View();
                }
                else
                {
                    return View("Authorization");
                }
            }
        }
        [AllowAnonymous]
        public IActionResult Main()
        {
            return View();
        }
        public RedirectToActionResult LeaveAccount()
        {
            CurrentUser.CurrentClientId = 0;
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Authorization");
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult PrivateAccount()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}