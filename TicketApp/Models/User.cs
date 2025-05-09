using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketApp.Models
{
    public class User
    {
        public int userId { get; set; }
       
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int userType { get; set; }
        public bool userActive { get; set; }
        public string emailAddress { get; set; }
        public string? phoneNumber { get; set; }
    
        public string passwordHash { get; set; }
        // foreign key : companyId
        public int companyId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        //add-migration initialmigration

    }

}
