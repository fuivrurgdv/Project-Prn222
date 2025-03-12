using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Project_MVC.Models.Auth;

namespace Project_MVC.Controllers
{
    public class AuthController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModelRequest());
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModelRequest request)
        {


            ////lỗi validation
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            // url API
            var apiUrl = "http://localhost:5260/api/Auth/login";
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(apiUrl, request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<APILoginResponse>();
                //gắn token vào session
                if (result != null)
                {
                    HttpContext.Session.SetString("JWToken", result.Token);
                }

                return RedirectToAction("Index", "Users");
            }
            else
            {
                ModelState.AddModelError("Erorr", "tên đăng nhập hoặc mật khẩu không chính xác");
                return View(request);
            }
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
