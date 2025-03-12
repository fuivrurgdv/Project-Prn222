using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Project_MVC.Models.Setting;

namespace Project_MVC.Controllers
{
    public class SettingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public SettingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;


        }


      


        // lấy các setting
        [HttpGet]
        public async Task<IActionResult> GetSetting()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new List<SettingDTO>();


            // -----Url lay thonog tin department tu uid----------


            var apiUrl = "http://localhost:5260/api/Setting/GếtttingList";

            // gọi api 
            var response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var departmentResponse = JsonSerializer.Deserialize<List<SettingDTO>>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model = departmentResponse;
                return View("Index", model);

            }
            else
            {
                return Content(apiUrl);
            }





        }

        // tạo mới 1 setting
        [HttpGet]
        public IActionResult CreateSetting()
        {
            return View(new SettingRequest());
        }

        // Tạo mới setting
        [HttpPost]
        public async Task<IActionResult> CreateSetting(SettingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API CreateUser
            var response = await client.PostAsync("http://localhost:5260/api/Setting/Add", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm thành công!";
                return RedirectToAction("GetSetting");
            }
            TempData["ErrorMessage"] = "Thêm thất bại!";
            return RedirectToAction("GetSetting");


        }

        // kích hoạt setting
        [HttpPut]
        public async Task<IActionResult> IActiveSetting(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------



            // -----Url lay thonog tin nhan vien tu uid----------
            var apiUrl = $"http://localhost:5260/api/Setting/isActive/{id}";



            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            // nếu mà thành công


            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "kích hoạt thành công!";
                return RedirectToAction("GetSetting");
            }
            TempData["ErrorMessage"] = "kích hoạt thất bại!";
            return RedirectToAction("GetSetting");


        }

        // khóa phòng setting
        [HttpPut]
        public async Task<IActionResult> DisabeSetting(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------



            // -----Url lay thonog tin nhan vien tu uid----------
            var apiUrl = $"http://localhost:5260/api/Setting/disable/{id}";



            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "khóa thành công!";
                return RedirectToAction("GetSetting");
            }
            TempData["ErrorMessage"] = "khóa thất bại!";
            return RedirectToAction("GetSetting");

        }

    }
}
