using System.ComponentModel.DataAnnotations;

namespace ShaTaskApp.Models.Business
{
    public class CreateProduct
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Category { get; set; }
    }
}