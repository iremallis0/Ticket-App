using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketApp.Models
{
    public class Ticket
    {
        public int ticketId { get; set; }
        public string subject { get; set; }
        public string detail { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? finishDate { get; set; }
        public string state { get; set; }
        public string severity { get; set; }

        public int categoryId { get; set; }
        public int userId { get; set; }
        [Range(0, Double.PositiveInfinity)]
        public int companyId { get; set; }
        public virtual User? User { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Company? Company { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }

    }
}
