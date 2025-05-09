using System.ComponentModel.DataAnnotations;

namespace TicketApp.Models
{
    public class SignInVM
    {
        [Required]
        [EmailAddress]
        public string emailAddress { get; set; }

        [Required]
        public string passwordText { get; set; }

        public bool rememberMe { get; set; }
    }
}
