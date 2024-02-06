using ConsumerWebAPIApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ConsumerWebAPIApp.Controllers
{
    public class ProductController : Controller
    {
        Uri base_address = new Uri("http://localhost:5124/api");
        private readonly HttpClient _client;
        ProductViewModel product = new ProductViewModel();
        public ProductController()
        {
            _client = new HttpClient();
            _client.BaseAddress = base_address;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/GetProducts").Result;
            // http://localhost:5124/api/Product/GetProducts
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
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, encoding: Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Product/PostProduct", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMsg"] = "Product Created";
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View();
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
              //  ProductViewModel product = new ProductViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/GetProductByID/" + Id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<ProductViewModel>(data);

                }
                return View(product);

            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View();

            }
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, encoding: Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Product/UpdateProduct", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMsg"] = "Product Updated";
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View();
            }
            return View();
        }


        [HttpGet]
        public IActionResult Details(int Id)
        {
           // ProductViewModel product = new ProductViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/GetProductByID/" + Id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductViewModel>(data);
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {

                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Product/GetProductByID/" + Id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<ProductViewModel>(data);

                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View();
            }

        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Product/DeleteProduct?id=" + Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMsg"] = "Product Deleted";
                    return RedirectToAction("Index");

                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View();
            }
        }


    }
}
