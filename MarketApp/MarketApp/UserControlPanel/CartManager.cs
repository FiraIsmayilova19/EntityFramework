using MarketLibrary;
using MarketLibrary.Data;
using System.Linq;

namespace UserPanel.UserControlPanel
{
    public class CartManager
    {
        private readonly AppDbContext _context;
        private readonly User _user;

        public CartManager(AppDbContext context, User user)
        {
            _context = context;
            _user = user;
        }

        public void AddToCart(Product product, int quantity)
        {
            var existingCart = _context.Carts.FirstOrDefault(c => c.ProductId == product.ProductId && c.UserId == _user.UserId);
            if (existingCart != null)
            {
                existingCart.Quantity += quantity;
            }
            else
            {
                var cart = new Cart
                {
                    ProductId = product.ProductId,
                    Product = product,
                    UserId = _user.UserId, 
                    Quantity = quantity
                };
                _context.Carts.Add(cart);
            }
            _context.SaveChanges();
        }

        public void RemoveProduct(int productId)
        {
            var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == _user.UserId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }
        }


        public double Checkout(double payAmount)
        {
            var cartItems = _context.Carts.ToList();
            double total = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            if (payAmount >= total)
            {
                _context.Carts.RemoveRange(cartItems);
                _context.SaveChanges();
                return payAmount - total;
            }

            throw new Exception("Odenish meblegi kifayet deyil.");
        }

        public List<Cart> GetCartItems()
        {
            return _context.Carts.ToList();
        }
    }
}
