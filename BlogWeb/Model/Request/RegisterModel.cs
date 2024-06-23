using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Model.Request
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email address is required")]
        public string? Email { get; set; }
    }
}