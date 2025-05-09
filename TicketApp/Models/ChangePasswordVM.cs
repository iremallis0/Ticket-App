namespace TicketApp.Models
{
    public class ChangePasswordVM
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}
