using HPlusSport.API.Models;
using HPlusSport.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public CategoryController(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _categoryService.GetAll().Select(c => new
            {
                c.Id,
                c.CategoryName,
                c.Description,
                c.CreatedAt,
                ProductCount = _categoryService.GetProductCount(c.Id, _productService.GetAll())
            });
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound($"Category not found with id {id}");

            var result = new
            {
                category.Id,
                category.CategoryName,
                category.Description,
                category.CreatedAt,
                ProductCount = _categoryService.GetProductCount(category.Id, _productService.GetAll())
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetProducts(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound($"Category not found with id {id}");

            var products = _productService.GetByCategory(id);
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Post(Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid category data");

            // Check if category name already exists
            if (_categoryService.GetByName(model.CategoryName) != null)
                return BadRequest($"Category with name '{model.CategoryName}' already exists");

            _categoryService.Add(model);
            return Ok("Category created successfully");
        }

        [HttpPut]
        public IActionResult Put(Category model)
        {
            if (model == null || model.Id == 0)
            {
                if (model == null)
                    return BadRequest("Model data invalid");
                else if (model.Id == 0)
                    return BadRequest($"Category Id {model.Id} is invalid");
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid category data");

            var existingCategory = _categoryService.GetById(model.Id);
            if (existingCategory == null)
                return NotFound($"Category not found with id {model.Id}");

            // Check if new name conflicts with existing category (excluding current)
            var categoryWithSameName = _categoryService.GetByName(model.CategoryName);
            if (categoryWithSameName != null && categoryWithSameName.Id != model.Id)
                return BadRequest($"Category with name '{model.CategoryName}' already exists");

            var updated = _categoryService.Update(model.Id, model);
            if (!updated)
                return NotFound($"Category not found with id {model.Id}");

            return Ok("Category updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound($"Category not found with id {id}");

            // Check if category has products
            if (_productService.HasProductsInCategory(id))
                return BadRequest("Cannot delete category with existing products. Please reassign or delete products first.");

            var deleted = _categoryService.Delete(id);
            if (!deleted)
                return NotFound($"Category not found with id {id}");

            return Ok("Category deleted successfully");
        }
    }
}