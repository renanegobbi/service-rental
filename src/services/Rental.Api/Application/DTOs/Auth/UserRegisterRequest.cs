using System.ComponentModel.DataAnnotations;

namespace Rental.Api.Application.DTOs.Auth
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "The {0} field is required")]
        [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(100, ErrorMessage = "The {0} field must be between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string PasswordConfirmation { get; set; }
    }
}
