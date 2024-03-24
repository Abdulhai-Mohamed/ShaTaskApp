using ShaTaskApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShaTaskApp.Models.Business
{
    public class CreateInvoice
    {
        public List<Product> Products;

        [Required]
        public bool Status { get; set; }

        [Required]
        public int? TaxRate { get; set; } // Decimal for tax rate (e.g., 0.075 for 7.5%)

        [Required]
        public int? TaxAmount { get; set; } // Calculated tax amount

        [Required]
        public int? Discount { get; set; } // Decimal representing any discount applied

        public string Notes { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerAddress { get; set; }

        [Required]
        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public List<InvoiceProduct> InvoiceProducts { get; set; }
    }
}