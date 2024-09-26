using MarketLibrary;
using MarketLibrary.Data;

namespace UserPanel.UserControlPanel
{
    public class CategoryManager
    {
        private readonly AppDbContext _context;

        public CategoryManager(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
