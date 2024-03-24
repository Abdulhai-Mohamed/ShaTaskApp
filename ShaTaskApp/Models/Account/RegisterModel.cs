using System.ComponentModel.DataAnnotations;

namespace ShaTaskApp.Models.Account
{
    public class RegisterModel
    {
        [Required,
        EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required,
        DataType(DataType.Password)]
        public string Password { get; set; }

        [Required,
        DataType(DataType.Password),
         Compare("Password", ErrorMessage = "yastaa Password and confirmation password do not match."),
         Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}