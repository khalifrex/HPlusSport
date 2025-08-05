using System.Text;
using HPlusSport.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HPlusSport.Client.Controllers
{
    public class CategoryController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5038/api");
        private readonly HttpClient _client;

        public CategoryController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CategoryViewModel> categoryList = new List<CategoryViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Category/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                categoryList = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);
            }
            return View(categoryList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Category/Post", content).Result;
            
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Category Created.";
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            CategoryViewModel category = new CategoryViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Category/Get/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<CategoryViewModel>(data);
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Category/Put", content).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Category Updated.";
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            CategoryViewModel category = new CategoryViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Category/Get/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<CategoryViewModel>(data);
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Category/Delete/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Category deleted successfully.";
            }
            else
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                TempData["errorMessage"] = errorMessage;
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Products(int id)
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            CategoryViewModel category = new CategoryViewModel();
            
            // Get category details
            HttpResponseMessage categoryResponse = _client.GetAsync(_client.BaseAddress + "/Category/Get/" + id).Result;
            if (categoryResponse.IsSuccessStatusCode)
            {
                string categoryData = categoryResponse.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<CategoryViewModel>(categoryData);
            }
            
            // Get products in category
            HttpResponseMessage productsResponse = _client.GetAsync(_client.BaseAddress + "/Category/GetProducts/" + id).Result;
            if (productsResponse.IsSuccessStatusCode)
            {
                string productsData = productsResponse.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(productsData);
            }
            
            ViewBag.Category = category;
            return View(productList);
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