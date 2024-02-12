using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackingSystem.Controllers.Requests
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

}
