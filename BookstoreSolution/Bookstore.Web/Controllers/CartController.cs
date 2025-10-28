using Bookstore.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bookstore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public CartController(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = config["ApiBaseUrl"] ?? "https://localhost:7144/api/";
        }

        public async Task<IActionResult> Index(int userId = 1)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}Cart/{userId}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse<CartViewModel>>(json);
            return View(result?.Data);
        }

        public async Task<IActionResult> Add(int userId, int bookId)
        {
            await _httpClient.PostAsync($"{_apiBaseUrl}Cart/add?userId={userId}&bookId={bookId}&quantity=1", null);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int userId, int bookId)
        {
            await _httpClient.DeleteAsync($"{_apiBaseUrl}Cart/remove?userId={userId}&bookId={bookId}");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PlaceOrder(int userId, string shippingAddress)
        {
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}Orders/place?userId={userId}&shippingAddress={shippingAddress}", null);

            if (response.IsSuccessStatusCode)
                return View("~/Views/Cart/Success.cshtml"); 

            return View("Error");
        }
    }
}
