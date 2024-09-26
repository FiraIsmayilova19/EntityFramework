using MarketLibrary;
using MarketLibrary.Data;

namespace AdminPanel.StockControlPanel
{
    internal class ProductManager
    {
        private readonly AppDbContext _context;

        public ProductManager(AppDbContext context)
        {
            _context = context;
        }

        public void AddProduct(string name, string description, int categoryId, double price, int quantity)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                CategoryId = categoryId,
                Price = price,
                Quantity = quantity
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
            if (product != null)
            {
                product.Name = updatedProduct.Name;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.Quantity = updatedProduct.Quantity;

                _context.SaveChanges();
            }
        }

        public void UpdateProductName(string oldName, string updatedProductName)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name == oldName);
            if (product != null)
            {
                product.Name = updatedProductName;
                _context.SaveChanges();
            }
        }

        public void UpdateProductPrice(string updatedProductName, double updatedProductPrice)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name == updatedProductName);
            if (product != null)
            {
                product.Price = updatedProductPrice;
                _context.SaveChanges();
            }
        }

        public void UpdateProductDescription(string updatedProductName, string updatedProductDescription)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name == updatedProductName);
            if (product != null)
            {
                product.Description = updatedProductDescription;
                _context.SaveChanges();
            }
        }

        public void UpdateProductQuantity(string updatedProductName, int updatedProductQuantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name == updatedProductName);
            if (product != null)
            {
                product.Quantity = updatedProductQuantity;
                _context.SaveChanges();
            }
        }
    }
}
