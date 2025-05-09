using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TicketApp.Models
{
    public class Company
    {
        public int companyId { get; set; }
        [Required]
        public string companyName { get; set; }
        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
