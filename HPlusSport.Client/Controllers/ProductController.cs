using System.Text;
using HPlusSport.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/Get").Result;

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
            ViewBag.Categories = GetCategoriesSelectList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategoriesSelectList();
                return View(model);
            }

            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Product/Post", content).Result;
            
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Product Created.";
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
                ViewBag.Categories = GetCategoriesSelectList();
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProductViewModel product = new ProductViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/Get/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductViewModel>(data);
            }

            ViewBag.Categories = GetCategoriesSelectList();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategoriesSelectList();
                return View(model);
            }

            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Product/Put", content).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Product Updated.";
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
                ViewBag.Categories = GetCategoriesSelectList();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ProductViewModel product = new ProductViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/Get/" + id).Result;
            
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
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Product/Delete/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Product deleted successfully.";
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/Search?searchTerm=" + Uri.EscapeDataString(searchTerm)).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
                }
            }
            
            ViewBag.SearchTerm = searchTerm;
            return View("Index", productList);
        }

        [HttpGet]
        public IActionResult UpdateStock(int id)
        {
            ProductViewModel product = new ProductViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/Get/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductViewModel>(data);
            }
            
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateStock(int id, int qty)
        {
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Product/UpdateStock/" + id + "/" + qty, null).Result;
            
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Product stock updated successfully.";
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
            }
            
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetCategoriesSelectList()
        {
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Category/Get").Result;
            
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);
            }
            
            return categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}