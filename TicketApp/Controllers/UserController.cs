using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using TicketApp.ActionFilters;
using TicketApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TicketApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbWorks _dbworks;
        public UserController(ILogger<UserController> logger, DbWorks dbworks)
        {
            _logger = logger;
            _dbworks = dbworks;
        }

        [SessionFilter]
        [PermissionFilter]
        public IActionResult Index()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            var loggedUser = _dbworks.Users.Find(userId);
            ViewBag.loggedUserFullName = loggedUser.firstName + " " + loggedUser.lastName;
            ViewBag.loggedUserEmailAddress = loggedUser.emailAddress;
            List<User> users = _dbworks.Users.ToList();
            return View(users);
          
        }


        [SessionFilter]
        [PermissionFilter]
        public IActionResult Create()
        {
            ViewBag.companies = _dbworks.Companies.ToList();
            return View();

        }



        [SessionFilter]
        [PermissionFilter]

        public IActionResult Display(int id)
        {
            User user = _dbworks.Users.Where(c => c.userId == id).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.companies = _dbworks.Companies.ToList();
            return View(user);
        }


        [SessionFilter]
        [PermissionFilter]
        [HttpPost]
        public IActionResult Store(User newUser)
        {
            if (ModelState.IsValid)
            {

                newUser.passwordHash= passwordHasher(newUser.passwordHash);
                _dbworks.Users.Add(newUser);
                _dbworks.SaveChanges();


                //return RedirectToAction("Index", "Company", newCompany.companyId);
                return RedirectToAction("Display", "User", new { id = newUser.userId });
            }
            ViewBag.companies = _dbworks.Companies.ToList();
            return View("Create", newUser);
        }



        [SessionFilter]
        [PermissionFilter]
        [HttpPost]
        public IActionResult Update(User newUser)
        {
            if (ModelState.IsValid)
            {
                User user = _dbworks.Users.Where(c => c.userId == newUser.userId).FirstOrDefault();
                if (user == null) return RedirectToAction("Index");

                user.firstName = newUser.firstName;
                user.lastName = newUser.lastName;
                user.userType = newUser.userType;
                user.userActive = newUser.userActive;
                user.emailAddress = newUser.emailAddress;
                if (user.passwordHash != newUser.passwordHash)
                {
                    user.passwordHash = passwordHasher(newUser.passwordHash);
                }
                user.phoneNumber = newUser.phoneNumber;
                user.companyId = newUser.companyId;
                _dbworks.SaveChanges();


                //return RedirectToAction("Index", "Company", newCompany.companyId);
                return RedirectToAction("Display", "User", new { id = newUser.userId });

            }
            ViewBag.companies = _dbworks.Companies.ToList();
            return View("Display", newUser);
        }



        [SessionFilter]
        [PermissionFilter]
        public IActionResult Delete(int id)
        {
            User user = _dbworks.Users.Where(c => c.userId == id).FirstOrDefault();
            if (user == null)
            {

                TempData["message"] = "DeleteFail";
                return RedirectToAction("Index", "User");

            }
            else
            {
                try
                {
                    _dbworks.Users.Remove(user);
                    _dbworks.SaveChanges();
                    TempData["message"] = "DeleteSuccess";
                    return RedirectToAction("Index", "User");
                }
                catch
                {
                    TempData["message"] = "DeleteRecordExists";
                    return RedirectToAction("Index", "User" +
                        "");
                }
            }
        }


        public string passwordHasher(string passwordText)
        {
            byte[] salt = Encoding.ASCII.GetBytes("Irem");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordText,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }




    }
}
