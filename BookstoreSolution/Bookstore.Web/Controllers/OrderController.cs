using Bookstore.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Orders/user/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Could not load your orders.";
                    return View(new List<OrderViewModel>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponse<List<OrderViewModel>>>(json);
                return View(result?.Data ?? new List<OrderViewModel>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading orders: {ex.Message}";
                return View(new List<OrderViewModel>());
            }
        }

    
        [HttpGet]
        public async Task<IActionResult> PlaceOrder(int userId = 1)
        {
            try
            {
                var cartResp = await _httpClient.GetAsync($"{_apiBaseUrl}Cart/{userId}");
                if (!cartResp.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Could not fetch cart. Please try again.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                var cartJson = await cartResp.Content.ReadAsStringAsync();
                var cartApi = JsonConvert.DeserializeObject<ApiResponse<CartViewModel>>(cartJson);
                var cart = cartApi?.Data;

                if (cart == null)
                {
                    TempData["Error"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

               
                return View("~/Views/Cart/Checkout.cshtml", cart);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Unexpected error: {ex.Message}";
                return RedirectToAction("Index", "Cart", new { userId });
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(int userId, string shippingAddress)
        {
            try
            {
              
                var cartResp = await _httpClient.GetAsync($"{_apiBaseUrl}Cart/{userId}");
                if (!cartResp.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Could not fetch cart. Please try again.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                var cartJson = await cartResp.Content.ReadAsStringAsync();
                var cartApi = JsonConvert.DeserializeObject<ApiResponse<CartViewModel>>(cartJson);
                var cart = cartApi?.Data;

                if (cart == null || cart.Items == null || !cart.Items.Any())
                {
                    TempData["Error"] = "Your cart is empty. Add items before placing an order.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                var orderDto = new
                {
                    UserId = userId,
                    ShippingAddress = shippingAddress ?? string.Empty,
                    Items = cart.Items.Select(i => new
                    {
                        BookId = i.BookId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                var payload = JsonConvert.SerializeObject(orderDto);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                
                var apiResponse = await _httpClient.PostAsync($"{_apiBaseUrl}Orders/place?userId={userId}", content);

                if (apiResponse.IsSuccessStatusCode)
                {
                    

                    return View("~/Views/Cart/Success.cshtml"); 
                }
                else
                {
                    var errorBody = await apiResponse.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Failed to place order: {apiResponse.StatusCode}. {errorBody}";
                    return RedirectToAction("Index", "Cart", new { userId });
                }
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = $"Network error while placing order: {httpEx.Message}";
                return RedirectToAction("Index", "Cart", new { userId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Unexpected error: {ex.Message}";
                return RedirectToAction("Index", "Cart", new { userId });
            }
        }
        
        [HttpGet]
        [Route("Order/Details/{id}")]
        [Route("Orders/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Orders/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Could not load order details.";
                    return RedirectToAction("MyOrders");
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResp = JsonConvert.DeserializeObject<ApiResponse<OrderViewModel>>(json);
                var model = apiResp?.Data;
                if (model == null) return RedirectToAction("MyOrders");

                return View("~/Views/Order/Details.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading order: {ex.Message}";
                return RedirectToAction("MyOrders");
            }
        }

        private class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public T Data { get; set; }
        }
    }

}
