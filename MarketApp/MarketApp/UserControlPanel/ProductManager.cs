using MarketLibrary;
using MarketLibrary.Data;
using System.Collections.Generic;
using System.Linq;

namespace UserPanel.UserControlPanel
{
    public class ProductManager
    {
        private readonly AppDbContext _context;

      
        public ProductManager(AppDbContext context)
        {
            _context = context;
        }

      
        public List<Product> GetProducts()
        {
            return _context.Products.ToList(); 
        }

        
        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

       
        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public Product GetProductByName(string productName)
        {
            return _context.Products.FirstOrDefault(p => p.Name == productName);
        }

        public void AddToCart(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null && product.Quantity >= quantity)
            {
               
                product.Quantity -= quantity;
                _context.SaveChanges(); 
            }
            else
            {
                throw new Exception("Mehsul movcud deyil ve ya yeterli miqdarda yoxdur.");
            }
        }

    
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                _context.SaveChanges();
            }
        }
        public void UpdateStock(int productId, int newStock)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.Quantity = product.Quantity - newStock;
                _context.SaveChanges();
            }
        }
    }
}
