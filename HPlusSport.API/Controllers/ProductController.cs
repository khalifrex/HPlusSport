using HPlusSport.API.Models;
using HPlusSport.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_service.GetAll());
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _service.GetById(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public IActionResult Post(Product model)
        {
            _service.Add(model);
            return Ok("Product created");
        }

        [HttpPut]
        public IActionResult Put(Product model)
        {
            if (model == null || model.Id == 0)
            {
                if (model == null)
                {
                    return BadRequest("model data invalid");
                }
                else if (model.Id == 0)
                {
                    return BadRequest($"Product Id {model.Id} is invalid");
                }
            }

            var product = _service.GetById(model.Id);
            if (product == null)
            {
                return NotFound($"Product not found with id {model.Id}");
            }
            product.ProductName = model.ProductName;
            product.Price = model.Price;
            product.Qty = model.Qty;
            return Ok("Product details updated");
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
            {
                return NotFound($"Product not found with id {id}");
            }
            _service.Delete(id);
            return Ok("Product details deleted");
       }

    }
}