using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketApp.Models;
namespace TicketApp.ActionFilters
{
    public class PermissionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            int result = Convert.ToInt32(context.HttpContext.Session.GetString("userType"));

            if (result != 2)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);

            }





        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
