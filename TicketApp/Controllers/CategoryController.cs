
using Microsoft.AspNetCore.Mvc;
using TicketApp.ActionFilters;
using TicketApp.Models;

namespace TicketApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly DbWorks _dbworks;

        public CategoryController(ILogger<CategoryController> logger, DbWorks dbworks)
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


            List<Category> categories = _dbworks.Categories.ToList();
            return View(categories);
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
            Category category = _dbworks.Categories.Where(c => c.categoryId == id).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            return View(category);

        }

        [SessionFilter]
        [PermissionFilter]
        [HttpPost]
        public IActionResult Store(Category newCategory)
        {
            if (ModelState.IsValid)
            {

                _dbworks.Categories.Add(newCategory);
                _dbworks.SaveChanges();


         
                return RedirectToAction("Display", "Category", new { id = newCategory.categoryId });
            }
            return View("Create", newCategory);
        }

        [SessionFilter]
        [PermissionFilter]
        [HttpPost]
        public IActionResult Update(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                Category category = _dbworks.Categories.Where(c => c.categoryId == newCategory.categoryId).FirstOrDefault();
                if (category == null) return RedirectToAction("Index");

                category.categoryName = newCategory.categoryName;
                _dbworks.SaveChanges();


                //return RedirectToAction("Index", "Company", newCompany.companyId);
                return RedirectToAction("Display", "Category", new { id = newCategory.categoryId });

            }
            return View("Display", newCategory);
        }

        [SessionFilter]
        [PermissionFilter]
        public IActionResult Delete(int id)
        {
            Category category = _dbworks.Categories.Where(c => c.categoryId == id).FirstOrDefault();
            if (category == null)
            {

                TempData["message"] = "DeleteFail";
                return RedirectToAction("Index", "Category");

            }
            else
            {
                try
                {
                    _dbworks.Categories.Remove(category);
                    _dbworks.SaveChanges();
                    TempData["message"] = "DeleteSuccess";
                    return RedirectToAction("Index", "Category");
                }
                catch
                {
                    TempData["message"] = "DeleteRecordExists";
                    return RedirectToAction("Index", "Category");
                }
            }
        }








    }
}
