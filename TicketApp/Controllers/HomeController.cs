using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using TicketApp.Models;
using TicketApp.ActionFilters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbWorks _dbworks;

        public HomeController(ILogger<HomeController> logger, DbWorks dbworks)
        {
            _logger = logger;
            _dbworks = dbworks;
        } 

        [SessionFilter]
        public IActionResult Index()
        {
          int userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            var loggedUser = _dbworks.Users.Find(userId);
            ViewBag.loggedUserFullName = loggedUser.firstName + " "+ loggedUser.lastName;
            ViewBag.loggedUserEmailAddress = loggedUser.emailAddress;

            var viewModel = new TicketComment();
            int userType = Convert.ToInt32(HttpContext.Session.GetString("userType"));

            if (userType == 1) // M��teri, sadece kendi taleplerini g�rs�n
            {
                viewModel.LastFiveTickets = _dbworks.Tickets
                    .Where(t => t.userId == userId) // Kullan�c�n�n ID'siyle filtrele
                    .Include(t => t.User)
                    .OrderByDescending(t => t.startDate)
                    .Take(5)
                    .ToList();


                // Kullan�c�n�n kendi taleplerine gelen yorumlar� al
                viewModel.LastFiveComments = _dbworks.Comments
                    .Where(c => c.Ticket.userId == userId) // Yorumun ait oldu�u talep kullan�c�n�n ID'siyle filtreleniyor
                    .Include(c => c.User)
                    .OrderByDescending(c => c.commentDate)
                    .Take(5)
                    .ToList();
            }
            else // Admin (2) veya di�erleri (0), t�m talepleri g�rs�n
            {
                viewModel.LastFiveTickets = _dbworks.Tickets
                    .Include(t => t.User)
                    .OrderByDescending(t => t.startDate)
                    .Take(5)
                    .ToList();

                viewModel.LastFiveComments = _dbworks.Comments
                    .Include(c => c.User)
                    .OrderByDescending(c => c.commentDate)
                    .Take(5)
                    .ToList();

            }
            // Son 5 talebi getirme
            //viewModel.LastFiveTickets = _dbworks.Tickets.Include(c => c.User)
            //    .OrderByDescending(t => t.startDate)
            //    .Take(5)
            //    .ToList();

            // Son 5 yorumu getirme
            //viewModel.LastFiveComments = _dbworks.Comments.Include(c=>c.User)
            //    .OrderByDescending(c => c.commentDate)
            //    .Take(5)
            //    .ToList();

            return View(viewModel);




        }
        [SessionFilter]
        public IActionResult Profile()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            var loggedUser = _dbworks.Users.Find(userId);
            ViewBag.loggedUserFullName = loggedUser.firstName + " " + loggedUser.lastName;
            ViewBag.loggedUserEmailAddress = loggedUser.emailAddress;


            return View(new UserUpdate { User=loggedUser});
        }
        [SessionFilter]
        public async Task<IActionResult> SignOut()
        {
            // Session'� temizleyin
            HttpContext.Session.Clear();
            // Kullan�c�y� ana sayfaya y�nlendirin
            return RedirectToAction("Index", "Login");
        }


        [SessionFilter]
        [HttpPost]
        public IActionResult ChangePassword(UserUpdate model)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                var userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));

                var user = _dbworks.Users.FirstOrDefault(u => u.userId == userId);

                if (user.passwordHash == passwordHasher(model.ChangePasswordVM.currentPassword)) 
                {
                    if(model.ChangePasswordVM.newPassword== model.ChangePasswordVM.confirmNewPassword)
                    {  //�ifreyi g�ncelle
                        user.passwordHash = passwordHasher(model.ChangePasswordVM.newPassword);
                        _dbworks.SaveChanges();
                        TempData["message"] = "ChangePasswordSuccess";
                        return RedirectToAction("Profile", "Home");
                    }
                }
            }
            TempData["message"] = "ChangePasswordFail";
            return RedirectToAction("Profile", "Home");
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

        [SessionFilter]
        public virtual IActionResult UpdateUser(UserUpdate newUser)
        {

            if (newUser.User!=null)
            {
                User user = _dbworks.Users.Where(c => c.userId == newUser.User.userId).FirstOrDefault();
                if (user == null) 
                    return RedirectToAction("Index");

                user.firstName = newUser.User.firstName;
                user.lastName = newUser.User.lastName;
                user.phoneNumber = newUser.User.phoneNumber;
                _dbworks.SaveChanges();

                return RedirectToAction("Profile", "Home");
            }
                return View("Profile", newUser);
        } 




    }
}
