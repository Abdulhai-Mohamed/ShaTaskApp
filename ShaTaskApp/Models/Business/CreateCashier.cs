using System.ComponentModel.DataAnnotations;

namespace ShaTaskApp.Models.Business
{
    public class CreateCashier
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}