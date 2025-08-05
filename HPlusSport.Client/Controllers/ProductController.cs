using System.Text;
using System.Text.Json.Serialization;
using HPlusSport.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HPlusSport.Client.Controllers
{
    public class ProductController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5038/api");
        private readonly HttpClient _client;

        public ProductController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/product/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
            }
            return View(productList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Product/Post", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Product Created.";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProductViewModel product = new ProductViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/product/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductViewModel>(data);
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Product/Put", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ProductViewModel product = new ProductViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/product/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductViewModel>(data);
            }
            return View(product);
        }
         [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/product/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Product details deleted";
                return RedirectToAction("Index");
            }
            return View();
    }
    }
    
} 