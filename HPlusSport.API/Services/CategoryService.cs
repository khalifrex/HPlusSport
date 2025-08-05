using HPlusSport.API.Models;

namespace HPlusSport.API.Services
{
    public class CategoryService
    {
        private readonly List<Category> _categories = new();

        public CategoryService()
        {
            // Seed some default categories
            _categories.AddRange(new[]
            {
                new Category { Id = 1, CategoryName = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = 2, CategoryName = "Clothing", Description = "Apparel and fashion items" },
                new Category { Id = 3, CategoryName = "Books", Description = "Books and literature" },
                new Category { Id = 4, CategoryName = "Sports", Description = "Sports equipment and accessories" }
            });
        }

        public IEnumerable<Category> GetAll() => _categories;

        public Category? GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);

        public Category? GetByName(string name) => _categories.FirstOrDefault(c => 
            c.CategoryName.Equals(name, StringComparison.OrdinalIgnoreCase));

        public bool CategoryExists(int id) => _categories.Any(c => c.Id == id);

        public void Add(Category category)
        {
            category.Id = _categories.Count > 0 ? _categories.Max(c => c.Id) + 1 : 1;
            category.CreatedAt = DateTime.Now;
            _categories.Add(category);
        }

        public bool Update(int id, Category updatedCategory)
        {
            var category = GetById(id);
            if (category == null) return false;

            category.CategoryName = updatedCategory.CategoryName;
            category.Description = updatedCategory.Description;

            return true;
        }

        public bool Delete(int id)
        {
            var category = GetById(id);
            if (category == null) return false;

            _categories.Remove(category);
            return true;
        }

        public int GetProductCount(int categoryId, IEnumerable<Product> products)
        {
            return products.Count(p => p.CategoryId == categoryId);
        }
    }
}