namespace ShaTaskApp.Models.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public int? TaxRate { get; set; } // Decimal for tax rate (e.g., 0.075 for 7.5%)
        public int? TaxAmount { get; set; } // Calculated tax amount
        public int? Discount { get; set; } // Decimal representing any discount applied

        public string Notes { get; set; }

        public Customer Customer { get; set; }

        public List<InvoiceProduct> InvoiceProducts { get; set; }
        public int TotalAmount { get; set; }

        public Invoice()
        {
            InvoiceProducts = new List<InvoiceProduct>(); ;
        }
    }

    public class InvoiceProduct
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public string ProductCategory { get; set; }
        public int Quantity { get; set; }
        public bool ProductDeleted { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}