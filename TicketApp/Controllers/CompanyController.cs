using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketApp.ActionFilters;
using TicketApp.Models;

namespace TicketApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly DbWorks _dbworks;

        public CompanyController(ILogger<CompanyController> logger, DbWorks dbworks)
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


            List<Company> companies = _dbworks.Companies.ToList();
            return View(companies);

        }
        [SessionFilter]
        [PermissionFilter]
        public IActionResult Create()
        {

            return View();

        }
        [SessionFilter]
        [PermissionFilter]
        public IActionResult Display(int id)
        {
        

                Company company = _dbworks.Companies.Where(c => c.companyId == id).FirstOrDefault();
                if (company == null)
                {
                    return RedirectToAction("Index");
                }

                return View(company);
            
           
        }
        [SessionFilter]
        [PermissionFilter]
        [HttpPost]
        public IActionResult Store(Company newCompany)
        {
            if (ModelState.IsValid)
            {

                _dbworks.Companies.Add(newCompany);
                _dbworks.SaveChanges();


                //return RedirectToAction("Index", "Company", newCompany.companyId);
                return RedirectToAction("Display", "Company", new { id = newCompany.companyId });
            }
            return View("Create", newCompany);
        }


        [SessionFilter]
        [PermissionFilter]
        [HttpPost]
        public IActionResult Update(Company newCompany)
        {
            if (ModelState.IsValid)
            {
                Company company = _dbworks.Companies.Where(c => c.companyId == newCompany.companyId).FirstOrDefault();
                if (company == null) return RedirectToAction("Index");

                company.companyName = newCompany.companyName;
                _dbworks.SaveChanges();


                //return RedirectToAction("Index", "Company", newCompany.companyId);
                return RedirectToAction("Display", "Company", new { id = newCompany.companyId });

            }
            return View("Display", newCompany);
        }

        [SessionFilter]
        [PermissionFilter]
        public IActionResult Delete(int id)
        {
            Company company = _dbworks.Companies.Where(c => c.companyId == id).FirstOrDefault();
            if (company == null)
            {

                TempData["message"] = "DeleteFail";
                return RedirectToAction("Index", "Company");

            }
            else
            {
                try
                {
                    _dbworks.Companies.Remove(company);
                    _dbworks.SaveChanges();
                    TempData["message"] = "DeleteSuccess";
                    return RedirectToAction("Index", "Company");
                }
                catch
                {
                    TempData["message"] = "DeleteRecordExists";
                    return RedirectToAction("Index", "Company");
                }
            }
        }




    }
}
