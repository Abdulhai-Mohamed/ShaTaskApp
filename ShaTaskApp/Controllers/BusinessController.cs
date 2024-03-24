using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models;
using ShaTaskApp.Models.Business;
using ShaTaskApp.Models.Entities;

namespace ShaTaskApp.Controllers
{
    [Authorize]
    public class BusinessController : Controller

    {
        private readonly ShaTaskDB ShaTaskDB;

        public BusinessController(ShaTaskDB shaTaskDB)
        {
            ShaTaskDB = shaTaskDB;
        }

        #region Cashier

        [HttpGet]
        public IActionResult CashiersList()
        {
            var model = ShaTaskDB.Cashiers;
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateCashier()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCashier(CreateCashier model)
        {
            if (ModelState.IsValid)
            {
                //1-
                Cashier Cashier = new Cashier
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                };

                //2-
                // Store user data in AspNetUsers database table
                var result = ShaTaskDB.Cashiers.Add(Cashier);
                ShaTaskDB.SaveChanges();
                return RedirectToAction("ViewCashier", "Business", new { Id = Cashier.Id });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ViewCashier(int Id)
        {
            var Cashier = ShaTaskDB.Cashiers.Find(Id);
            if (Cashier == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Cashier Found by this id!";
                return View("AlertView");
            }
            return View(Cashier);
        }

        [HttpGet]
        public IActionResult UpdateCashier(int Id)
        {
            var Cashier = ShaTaskDB.Cashiers.Find(Id);
            if (Cashier == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Cashier Found by this id!";
                return View("AlertView");
            }
            return View(Cashier);
        }

        [HttpPost]
        public IActionResult UpdateCashier(Cashier model)
        {
            var Cashier = ShaTaskDB.Cashiers.Find(model.Id);
            Cashier.FirstName = model.FirstName;
            Cashier.LastName = model.LastName;
            Cashier.PhoneNumber = model.PhoneNumber;
            ShaTaskDB.Cashiers.Update(Cashier);
            ShaTaskDB.SaveChanges();
            return RedirectToAction("ViewCashier", "Business", new { Id = Cashier.Id });
        }

        [HttpPost]
        public IActionResult DeleteCashier(int Id)
        {
            var Cashier = ShaTaskDB.Cashiers.Find(Id);
            if (Cashier == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Cashier Found by this id!";
                return View("AlertView");
            }
            ShaTaskDB.Cashiers.Remove(Cashier);
            ShaTaskDB.SaveChanges();
            return RedirectToAction("CashiersList");
        }

        #endregion Cashier

        #region Product

        [HttpGet]
        public IActionResult ProductsList()
        {
            var model = ShaTaskDB.Products;
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProduct model)
        {
            if (ModelState.IsValid)
            {
                string categoryDisplayName = Enum.GetName(typeof(ProductCategories), Int32.Parse(model.Category));

                //1-
                Product Product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Category = categoryDisplayName,
                };

                //2-
                // Store user data in AspNetUsers database table
                var result = ShaTaskDB.Products.Add(Product);
                ShaTaskDB.SaveChanges();
                return RedirectToAction("ViewProduct", "Business", new { Id = Product.Id });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ViewProduct(int Id)
        {
            var Product = ShaTaskDB.Products.Find(Id);

            if (Product == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Product Found by this id!";
                return View("AlertView");
            }
            return View(Product);
        }

        [HttpGet]
        public IActionResult UpdateProduct(int Id)
        {
            var Product = ShaTaskDB.Products.Find(Id);

            if (Product == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Product Found by this id!";
                return View("AlertView");
            }
            return View(Product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product model)
        {
            var Product = ShaTaskDB.Products.Find(model.Id);
            string categoryDisplayName = Enum.GetName(typeof(ProductCategories), Int32.Parse(model.Category));

            Product.Name = model.Name;
            Product.Description = model.Description;
            Product.Price = model.Price;
            Product.Category = categoryDisplayName;
            ShaTaskDB.Products.Update(Product);
            ShaTaskDB.SaveChanges();
            return RedirectToAction("ViewProduct", "Business", new { Id = Product.Id });
        }

        [HttpPost]
        public IActionResult DeleteProduct(int Id)
        {
            var Product = ShaTaskDB.Products.Find(Id);
            if (Product == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Products Found by this id!";
                return View("AlertView");
            }
            ShaTaskDB.Products.Remove(Product);
            ShaTaskDB.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        #endregion Product

        #region Invoice

        [HttpGet]
        public IActionResult InvoicesList()
        {
            var model = ShaTaskDB.Invoices
                .Include(I => I.Customer)
                .Include(I => I.InvoiceProducts); // Include the new property
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateInvoice()
        {
            var Products = ShaTaskDB.Products.ToList();
            var model = new CreateInvoice()
            {
                Status = true,
                InvoiceProducts = new List<InvoiceProduct>(),
                Products = Products
            };

            foreach (Product p in Products)
            {
                var invoiceProduct = new InvoiceProduct()
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    ProductDescription = p.Description,
                    ProductCategory = p.Category,
                    Quantity = 0,
                };
                model.InvoiceProducts.Add(invoiceProduct);
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoice model)
        {
            var Products = ShaTaskDB.Products.ToList();

            if (ModelState.IsValid)
            {
                var now = DateTime.Now;

                //1-
                Invoice invoice = new Invoice
                {
                    Date = now,
                    Status = model.Status,
                    TaxRate = model.TaxRate,
                    TaxAmount = model.TaxAmount,
                    Discount = model.Discount,
                    Notes = model.Notes,
                    InvoiceProducts = model.InvoiceProducts,
                };

                string _InvoiceNumber = $"{invoice.Id}-{now.Year:0000}-{now.Month:00}-{invoice.TotalAmount}";
                invoice.InvoiceNumber = _InvoiceNumber;
                if (model.InvoiceProducts.Count > 0)
                {
                    if (model.InvoiceProducts.Any(p => p.Quantity > 0))
                    {
                        invoice.InvoiceProducts.ForEach(p =>
                        {
                            if (p.Quantity > 0)
                            {
                                var currentProduct = Products.Find(pro => pro.Id == p.ProductId);
                                var totalProductPrice = currentProduct.Price * p.Quantity;
                                invoice.TotalAmount = invoice.TotalAmount + totalProductPrice;
                            }
                        });
                    }
                    else
                    {
                        ViewBag.AlertType = "danger";

                        ViewBag.AlertTitle = "Missing Products";
                        ViewBag.AlertMessage = "No Products Added, Please Add some product to the Invoice";
                        return View("AlertView");
                    }
                    invoice.InvoiceProducts = invoice.InvoiceProducts.Where(Product => Product.Quantity > 0).ToList();

                    Customer customer = new Customer
                    {
                        Name = model.CustomerName,
                        Address = model.CustomerAddress,
                        Email = model.CustomerEmail,
                        Phone = model.CustomerPhone,
                    };
                    invoice.Customer = customer;

                    //2-
                    // Store user data in AspNetUsers database table
                    var result = ShaTaskDB.Invoices.Add(invoice);
                    ShaTaskDB.SaveChanges();
                    return RedirectToAction("ViewInvoice", "Business", new { Id = invoice.Id });
                }
                else
                {
                    ViewBag.AlertType = "danger";

                    ViewBag.AlertTitle = "Missing Products";
                    ViewBag.AlertMessage = "No Products Found, Please Create some product to then add them to Invoice";
                    return View("AlertView");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ViewInvoice(int Id)
        {
            var Invoice = ShaTaskDB.Invoices
                .Include(I => I.Customer)
                .Include(I => I.InvoiceProducts) // Include the new property
                .SingleOrDefault(i => i.Id == Id);

            if (Invoice == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Invoice Found by this id!";
                return View("AlertView");
            }
            var Products = ShaTaskDB.Products.ToList();

            foreach (var ip in Invoice.InvoiceProducts)
            {
                var pro = Products.Find(p => p.Id == ip.ProductId);
                if (pro is null) ip.ProductDeleted = true;
            }
            //update product delted prop
            ShaTaskDB.Invoices.Update(Invoice);
            ShaTaskDB.SaveChanges();

            return View(Invoice);
        }

        [HttpGet]
        public IActionResult UpdateInvoice(int Id)
        {
            var Invoice = ShaTaskDB.Invoices.
                Include(I => I.Customer)
                .Include(I => I.InvoiceProducts) // Include the new property
                .SingleOrDefault(i => i.Id == Id);

            if (Invoice == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Invoice Found by this id!";
                return View("AlertView");
            }
            var Products = ShaTaskDB.Products.ToList();

            foreach (var ip in Invoice.InvoiceProducts)
            {
                var pro = Products.Find(p => p.Id == ip.ProductId);
                if (pro is null) ip.ProductDeleted = true;
            }
            foreach (Product p in Products)
            {
                if (!Invoice.InvoiceProducts.Any(ip => ip.ProductId == p.Id))
                {
                    var invoiceProduct = new InvoiceProduct()
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        ProductPrice = p.Price,
                        ProductDescription = p.Description,
                        ProductCategory = p.Category,
                        Quantity = 0,
                    };
                    Invoice.InvoiceProducts.Add(invoiceProduct);
                }
            };
            return View(Invoice);
        }

        [HttpPost]
        public IActionResult UpdateInvoice(Invoice model)
        {
            var Products = ShaTaskDB.Products.ToList();

            var Invoice = ShaTaskDB.Invoices
                .Include(I => I.Customer)
                .Include(I => I.InvoiceProducts) // Include the new property
                .SingleOrDefault(i => i.Id == model.Id);

            Invoice.Status = model.Status;
            Invoice.TaxRate = model.TaxRate;
            Invoice.TaxAmount = model.TaxAmount;
            Invoice.Discount = model.Discount;
            Invoice.Notes = model.Notes;
            Invoice.InvoiceProducts = model.InvoiceProducts;

            if (model.InvoiceProducts.Any(ip => ip.ProductDeleted == false))
            {
                if (model.InvoiceProducts.Any(p => p.Quantity > 0))
                {
                    Invoice.TotalAmount = 0;
                    Invoice.InvoiceProducts.ForEach(ip =>
                    {
                        if (ip.Quantity > 0)
                        {
                            if (ip.ProductDeleted)
                            {
                                Invoice.TotalAmount = Invoice.TotalAmount + ip.ProductPrice * ip.Quantity;
                            }
                            else
                            {
                                var currentProduct = Products.Find(pro => pro.Id == ip.ProductId);
                                var totalProductPrice = currentProduct.Price * ip.Quantity;
                                Invoice.TotalAmount = Invoice.TotalAmount + totalProductPrice;
                            }
                        }
                    });
                }
                else
                {
                    ViewBag.AlertType = "danger";

                    ViewBag.AlertTitle = "Missing Products";
                    ViewBag.AlertMessage = "No Products Added, Please Add some product to the Invoice";
                    return View("AlertView");
                }
                Invoice.InvoiceProducts = Invoice.InvoiceProducts.Where(Product => Product.Quantity > 0).ToList();

                Invoice.Customer.Name = model.Customer.Name;
                Invoice.Customer.Address = model.Customer.Address;
                Invoice.Customer.Email = model.Customer.Email;
                Invoice.Customer.Phone = model.Customer.Phone;

                //2-

                ShaTaskDB.Invoices.Update(Invoice);
                ShaTaskDB.SaveChanges();
                return RedirectToAction("ViewInvoice", "Business", new { Id = Invoice.Id });
            }
            else
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "Missing Products";
                ViewBag.AlertMessage = "No Products Added, and the invoice products are old and deleted before,  Please Add some product to the Invoice";
                return View("AlertView");
            }
        }

        [HttpPost]
        public IActionResult DeleteInvoice(int Id)
        {
            var Invoice = ShaTaskDB.Invoices.Find(Id);
            if (Invoice == null)
            {
                ViewBag.AlertType = "danger";

                ViewBag.AlertTitle = "404!";
                ViewBag.AlertMessage = "No Invoice Found by this id!";
                return View("AlertView");
            }
            ShaTaskDB.Invoices.Remove(Invoice);
            ShaTaskDB.SaveChanges();
            return RedirectToAction("InvoicesList");
        }

        #endregion Invoice
    }
}