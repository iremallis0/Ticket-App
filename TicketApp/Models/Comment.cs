namespace TicketApp.Models
{
    public class Comment
    {
        public int commentId { get; set; }
        public string commentText { get; set; }
        public DateTime commentDate { get; set; }
        public int ticketId { get; set; }
        public int userId { get; set; }
        public virtual Ticket? Ticket { get; set; }
        public virtual User? User { get; set; }

    }
}
