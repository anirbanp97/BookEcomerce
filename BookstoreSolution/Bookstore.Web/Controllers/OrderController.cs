using Bookstore.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Bookstore.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public OrderController(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = config["ApiBaseUrl"] ?? "https://localhost:7144/api/";
        }

        public async Task<IActionResult> MyOrders(int userId = 1)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}Orders/user/{userId}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse<List<OrderViewModel>>>(json);
            return View(result?.Data);
        }

        public async Task<IActionResult> PlaceOrder(int userId = 1)
        {
            var orderData = new { UserId = userId, ShippingAddress = "Kolkata, India", Items = new List<object>() };
            var json = JsonConvert.SerializeObject(orderData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{_apiBaseUrl}Orders/place?userId={userId}", content);
            return RedirectToAction("MyOrders");
        }
        //public async Task<IActionResult> PlaceOrder(int userId = 1)
        //{
        //    var orderData = new { UserId = userId, ShippingAddress = "Kolkata, India" };
        //    var json = JsonConvert.SerializeObject(orderData);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync($"{_apiBaseUrl}Orders/place?userId={userId}", content);

        //    if (response.IsSuccessStatusCode)
        //        return View("Success"); 

        //    return View("Error");
        //}

    }
}
