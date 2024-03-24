using System.ComponentModel.DataAnnotations;

namespace ShaTaskApp.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        //is the URL the user was trying to access before authentication.We preserve and pass it between requests using ReturnUrl property, so the user can be redirected to that URL upon successful authentication.
        public string ReturnUrl { get; set; }
    }
}