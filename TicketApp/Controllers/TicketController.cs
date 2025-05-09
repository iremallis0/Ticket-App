using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TicketApp.Models;

namespace TicketApp.Controllers
{
    public class TicketController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbWorks _dbworks;
        public TicketController(ILogger<HomeController> logger, DbWorks dbworks)
        {
            _logger = logger;
            _dbworks = dbworks;
        }
        public IActionResult Index()
        {
          

            List<Ticket> tickets = _dbworks.Tickets.Include(t=> t.Company).ToList();
            return View(tickets);
        }

        public IActionResult Create()
        { 

            ViewBag.categories = _dbworks.Categories.ToList();
            ViewBag.companies = _dbworks.Companies.ToList();
         
            return View();
        }

        [HttpGet]
        public JsonResult GetUsersByCompany(int companyId)
        {
            var users = _dbworks.Users
                         .Where(u => u.companyId == companyId)
                         .Select(u => new SelectListItem
                         {
                             Value = u.userId.ToString(),
                             Text = u.firstName + " " + u.lastName
                         }).ToList();

            return Json(users);
        }



        public IActionResult Display(int id)
        {
            Ticket ticket = _dbworks.Tickets.Where(t => t.ticketId == id).Include(t=> t.User).Include(t => t.Company).Include(t=> t.Comments).ThenInclude(u=>u.User).FirstOrDefault();
            if (ticket == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.categories = _dbworks.Categories.ToList();
            return View(new TicketComment { Ticket= ticket });
        }

        [HttpPost]
        public IActionResult Store(Ticket newTicket)
        {
            if (ModelState.IsValid)
            {
            

                _dbworks.Tickets.Add(newTicket);
                _dbworks.SaveChanges();

                return RedirectToAction("Display", "Ticket", new { id = newTicket.ticketId });
            }
            
            ViewBag.categories = _dbworks.Categories.ToList();
            
            return View("Create", newTicket);
        }

        [HttpPost]
        public IActionResult Update(TicketComment newTicket)
        {
            ModelState.Remove("Comment");
            if (ModelState.IsValid)
            {
                Ticket ticket= _dbworks.Tickets.Where(c => c.ticketId == newTicket.Ticket.ticketId).FirstOrDefault();
                if (ticket == null) return RedirectToAction("Index");

                ticket.subject = newTicket.Ticket.subject;
                ticket.detail = newTicket.Ticket.detail;
                ticket.startDate = newTicket.Ticket.startDate;
                ticket.finishDate = newTicket.Ticket.finishDate;
                ticket.state = newTicket.Ticket.state;
                ticket.severity=newTicket.Ticket.severity;
                ticket.categoryId = newTicket.Ticket.categoryId;
                ticket.userId = newTicket.Ticket.userId;
                ticket.companyId = newTicket.Ticket.companyId;
                
                _dbworks.SaveChanges();


                return RedirectToAction("Display", "Ticket", new { id = newTicket.Ticket.ticketId });

            }
          
            ViewBag.categories = _dbworks.Categories.ToList();
           
            return View("Display", newTicket); 

        }


        public IActionResult Delete(int id)
        {
            Ticket ticket = _dbworks.Tickets.Where(c => c.ticketId == id).FirstOrDefault();
            if (ticket == null)
            {
                TempData["message"] = "DeleteFail";
                return RedirectToAction("Index", "Ticket");
            }
            else
            {
                try
                {
                    _dbworks.Tickets.Remove(ticket);
                    _dbworks.SaveChanges();
                    TempData["message"] = "DeleteSuccess";
                    return RedirectToAction("Index", "Ticket");
                }
                catch
                {
                    TempData["message"] = "DeleteRecordExists";
                    return RedirectToAction("Index", "Ticket" +"");
                }
            }
        }
        [HttpPost]
        public IActionResult ticketAdd(TicketComment newTComment )
        {
            if (newTComment.Comment!=null && newTComment.Comment.commentText != null)
            {
                newTComment.Comment.commentDate = DateTime.Now;
              
                newTComment.Comment.userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
                newTComment.Comment.ticketId = newTComment.Ticket.ticketId;
                _dbworks.Comments.Add(newTComment.Comment);
                _dbworks.SaveChanges();

                return RedirectToAction("Display", "Ticket", new { id = newTComment.Ticket.ticketId });
            }
            return View("Display",newTComment);
            
        }

        public IActionResult ticketDelete(int id) {

            Comment comment = _dbworks.Comments.Where(c => c.commentId == id).FirstOrDefault();
            _dbworks.Comments.Remove(comment);
            _dbworks.SaveChanges();
            TempData["message"] = "DeleteSuccess";
            return RedirectToAction("Display", "Ticket",  new { id = comment.ticketId });
        
        }






    }
} 

