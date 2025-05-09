using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using TicketApp.Models;

namespace TicketApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly DbWorks _dbworks;
        public LoginController(ILogger<LoginController> logger,DbWorks dbworks)
        {
            _logger = logger;
            _dbworks = dbworks;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SignInVM model)
        {
            if (ModelState.IsValid)
            {
                var loginUser = _dbworks.Users.Where(u => u.emailAddress == model.emailAddress).FirstOrDefault();
                if (loginUser != null && loginUser.passwordHash  == passwordHasher(model.passwordText) )
                {
                    HttpContext.Session.SetString("userType", loginUser.userType.ToString());
                    HttpContext.Session.SetString("userId", loginUser.userId.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (loginUser == null)
                    {
                        ViewBag.errorText = "Kullanıcı Bulunamadı!";
                    }
                    else
                    {
                        ViewBag.errorText = "Parola Hatalı!";
                    }

                    //hata bildir
                }
                // Kullanıcı giriş işlemleri
            }

            return View(model);
        }

        //parola hashleme
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
