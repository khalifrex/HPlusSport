using HPlusSport.API.Models;

namespace HPlusSport.API.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new();
        private readonly CategoryService _categoryService;

        public ProductService(CategoryService categoryService)
        {
            _categoryService = categoryService;
            
            // Seed some default products with categories
            _products.AddRange(new[]
            {
                new Product { Id = 1, ProductName = "iPhone 15", Price = 999.99m, Qty = 10, CategoryId = 1 },
                new Product { Id = 2, ProductName = "Samsung TV", Price = 799.99m, Qty = 5, CategoryId = 1 },
                new Product { Id = 3, ProductName = "Nike Shoes", Price = 129.99m, Qty = 20, CategoryId = 2 },
                new Product { Id = 4, ProductName = "Programming Book", Price = 49.99m, Qty = 15, CategoryId = 3 }
            });
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.Select(p => new Product
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                Qty = p.Qty,
                CategoryId = p.CategoryId,
                CreatedAt = p.CreatedAt,
                Category = _categoryService.GetById(p.CategoryId)
            });
        }

        public Product? GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return null;

            return new Product
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                Category = _categoryService.GetById(product.CategoryId)
            };
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new Product
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Qty = p.Qty,
                    CategoryId = p.CategoryId,
                    CreatedAt = p.CreatedAt,
                    Category = _categoryService.GetById(p.CategoryId)
                });
        }

        public IEnumerable<Product> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAll();

            return _products
                .Where(p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(p => new Product
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Qty = p.Qty,
                    CategoryId = p.CategoryId,
                    CreatedAt = p.CreatedAt,
                    Category = _categoryService.GetById(p.CategoryId)
                });
        }

        public bool Add(Product product)
        {
            // Validate category exists
            if (!_categoryService.CategoryExists(product.CategoryId))
                return false;

            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            product.CreatedAt = DateTime.Now;
            _products.Add(product);
            return true;
        }

        public bool Update(int id, Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            // Validate category exists
            if (!_categoryService.CategoryExists(updatedProduct.CategoryId))
                return false;

            product.ProductName = updatedProduct.ProductName;
            product.Price = updatedProduct.Price;
            product.Qty = updatedProduct.Qty;
            product.CategoryId = updatedProduct.CategoryId;

            return true;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }

        public bool UpdateStock(int id, int newQty)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            product.Qty = newQty;
            return true;
        }

        // Helper method for category service to check if category has products
        public bool HasProductsInCategory(int categoryId)
        {
            return _products.Any(p => p.CategoryId == categoryId);
        }
    }
}