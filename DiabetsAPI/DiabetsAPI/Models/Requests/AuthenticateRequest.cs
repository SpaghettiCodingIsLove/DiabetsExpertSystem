using System.ComponentModel.DataAnnotations;

namespace DiabetsAPI.Models.Requests
{
    public class AuthenticateRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
