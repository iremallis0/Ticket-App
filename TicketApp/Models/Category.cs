namespace TicketApp.Models
{
    public class Category
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
