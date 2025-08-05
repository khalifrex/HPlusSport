using HPlusSport.API.Models;
using HPlusSport.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_productService.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productService.GetById(id);
            return product is null ? NotFound($"Product not found with id {id}") : Ok(product);
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            var category = _categoryService.GetById(categoryId);
            if (category == null)
                return NotFound($"Category not found with id {categoryId}");

            var products = _productService.GetByCategory(categoryId);
            return Ok(products);
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term cannot be empty");

            var products = _productService.Search(searchTerm);
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Post(Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid product data");

            // Validate category exists
            if (!_categoryService.CategoryExists(model.CategoryId))
                return BadRequest($"Category with id {model.CategoryId} does not exist");

            var added = _productService.Add(model);
            if (!added)
                return BadRequest("Failed to create product");

            return Ok("Product created successfully");
        }

        [HttpPut]
        public IActionResult Put(Product model)
        {
            if (model == null || model.Id == 0)
            {
                if (model == null)
                    return BadRequest("Model data invalid");
                else if (model.Id == 0)
                    return BadRequest($"Product Id {model.Id} is invalid");
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid product data");

            var product = _productService.GetById(model.Id);
            if (product == null)
                return NotFound($"Product not found with id {model.Id}");

            // Validate category exists
            if (!_categoryService.CategoryExists(model.CategoryId))
                return BadRequest($"Category with id {model.CategoryId} does not exist");

            var updated = _productService.Update(model.Id, model);
            if (!updated)
                return NotFound($"Product not found with id {model.Id}");

            return Ok("Product updated successfully");
        }

        [HttpPut("{id}/{qty}")]
        public IActionResult UpdateStock(int id, int qty)
        {
            if (qty < 0)
                return BadRequest("Quantity cannot be negative");

            var product = _productService.GetById(id);
            if (product == null)
                return NotFound($"Product not found with id {id}");

            var updated = _productService.UpdateStock(id, qty);
            if (!updated)
                return NotFound($"Product not found with id {id}");

            return Ok("Product stock updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound($"Product not found with id {id}");

            var deleted = _productService.Delete(id);
            if (!deleted)
                return NotFound($"Product not found with id {id}");

            return Ok("Product deleted successfully");
        }
    }
}