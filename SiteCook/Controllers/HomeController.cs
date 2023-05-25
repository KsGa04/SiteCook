using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteCook.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SiteCook.Controllers
{
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
            if (role == "Администратор") {
                using (CookingBookContext db = new CookingBookContext())
                {
                    Administrator? administrator = (from o in db.Administrators where o.Mail == loginOrEmail && o.Password == password select o).FirstOrDefault();
                    if (administrator is not null)
                    {
                        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, administrator.IdAdministrator.ToString()), new Claim(ClaimsIdentity.DefaultRoleClaimType, "Administrator") };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("AddingCategory");
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
                        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, moderator.IdModerator.ToString()), new Claim(ClaimsIdentity.DefaultRoleClaimType, "Moderator")};
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("PrivateAccountModer");
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
                    SiteCook.User client = (from c in db.Users where (c.Mail == loginOrEmail) && c.Password == password select c).FirstOrDefault();
                    //HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (client is not null)
                    {
                        
                        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, client.IdUser.ToString()),
                        new Claim(ClaimTypes.Email, "testc@mail.ru"), new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")};
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Main");
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
                    User user = new User();
                    user.Mail = email;
                    user.Password = password;
                    db.Add(user);
                    db.SaveChanges();
                    return View("Authorization");
                }
            }
        }
        [AllowAnonymous]
        
        public RedirectToActionResult LeaveAccount()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Authorization");
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult Main()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult PrivateAccount()
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(user.Value);
            using (CookingBookContext db = new CookingBookContext())
            {
                User currentUser = db.Users.Where(e => e.IdUser == id).FirstOrDefault();
                return View("PrivateAccount", currentUser);
            }
            
        }
        public RedirectToActionResult SaveChangesUser(string loginOrEmail, string password, string nik, DateTime Data)
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(claim.Value);
            using (CookingBookContext db = new CookingBookContext())
            {
                User currentUser = db.Users.Where(e => e.IdUser == id).FirstOrDefault();
                currentUser.Mail = loginOrEmail;
                currentUser.Password = password;
                currentUser.NikName = nik;
                currentUser.DateOfBirth = Data;
                db.SaveChanges();
            }
            
            ViewBag.Message = string.Format("Данные были сохранены");
            return RedirectToAction("PrivateAccount");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult PrivateAccountModer()
        {
            var moder = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(moder.Value);
            using (CookingBookContext db = new CookingBookContext())
            {
                Moderator currentModerator = db.Moderators.Where(e => e.IdModerator == id).FirstOrDefault();
                return View("PrivateAccountModer", currentModerator);
            }

        }
        public RedirectToActionResult SaveChangesModer(string loginOrEmail, string password, string nik, DateTime Data)
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(claim.Value);
            using (CookingBookContext db = new CookingBookContext())
            {
                Moderator currentModer = db.Moderators.Where(e => e.IdModerator == id).FirstOrDefault();
                currentModer.Mail = loginOrEmail;
                currentModer.Password = password;
                currentModer.NikName = nik;
                currentModer.DateOfBirth = Data;
                db.SaveChanges();
            }

            ViewBag.Message = string.Format("Данные были сохранены");
            return RedirectToAction("PrivateAccountModer");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddingCategory()
        {
            return View();
        }
        public RedirectToActionResult AddCategory(string nameCategory, IFormFile photo)
        {
            MemoryStream ms = new MemoryStream();
            photo.CopyTo(ms);
            using (CookingBookContext db = new CookingBookContext())
            {
                Category category = new Category();
                category.NameCategory = nameCategory;
                category.ImageCategory = ms.ToArray();
                db.Add(category);
                db.SaveChanges();
            }
            return RedirectToAction("AddingCategory");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult AddingMeal()
        {
            return View();
        }
        public RedirectToActionResult AddMeal(string nameMeal, string desc, string namecateg, IFormFile photo)
        {
            MemoryStream ms = new MemoryStream();
            photo.CopyTo(ms);
            using (CookingBookContext db = new CookingBookContext())
            {
                var categ = db.Categories.Where(x => x.NameCategory == namecateg).FirstOrDefault();
                int id = categ.IdCategory;
                Meal meal = new Meal();
                meal.NameMeal = nameMeal;
                meal.ImageMeal = ms.ToArray();
                meal.DescriptionMeal = desc;
                meal.IdCategory = id;
                db.Add(meal);
                db.SaveChanges();
            }
            return RedirectToAction("AddingMeal");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult AddingModerator()
        {
            return View();
        }
        public RedirectToActionResult AddModerator(string email, string pass, string nickname, DateTime Data, string namecateg)
        {
            using (CookingBookContext db = new CookingBookContext())
            {
                var categ = db.Categories.Where(x => x.NameCategory == namecateg).FirstOrDefault();
                int id = categ.IdCategory;
                Moderator moderator = new Moderator();
                moderator.Mail = email;
                moderator.Password = pass;
                moderator.NikName = nickname;
                moderator.DateOfBirth = Data;
                moderator.IdCategory = id;
                db.Add(moderator);
                db.SaveChanges();
            }
            return RedirectToAction("AddingModerator");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}