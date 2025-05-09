using System.ComponentModel.DataAnnotations;

namespace TicketApp.Models
{
    public class TicketComment
    {
        public Ticket Ticket { get; set; }
        public Comment Comment { get; set; }
        public Company Company { get; set; }
        public List<Ticket>? LastFiveTickets { get; set; } // Son 5 Talep
        public List<Comment>? LastFiveComments { get; set; } // Son 5 Yorum

        //public int ticketId { get; set; }
        //public string subject { get; set; }
        //public string detail { get; set; }
        //public DateTime startDate { get; set; }
        //public DateTime? finishDate { get; set; }
        //public string state { get; set; }
        //public string severity { get; set; }

        //public int categoryId { get; set; }

        //[Range(0, Double.PositiveInfinity)]
        //public int companyId { get; set; }
        //public virtual User? User { get; set; }
        //public virtual Category? Category { get; set; }
        //public virtual Company? Company { get; set; }
        //public virtual ICollection<Comment>? Comments { get; set; }
        //public int commentId { get; set; }
        //public string commentText { get; set; }
        //public DateTime commentDate { get; set; }

        //public int userId { get; set; }
        //public virtual Ticket? Ticket { get; set; }

    }
}
