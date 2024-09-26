using MarketLibrary;
using MarketLibrary.Data;

namespace AdminPanel.StockControlPanel
{
    internal class CategoryManager
    {
        private readonly AppDbContext _context;

        public CategoryManager(AppDbContext context)
        {
            _context = context;
        }

        public void AddCategory(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
