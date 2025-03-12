using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Project_MVC.Models.Attendence;
using Project_MVC.Models.Setting;
using Project_MVC.Models.APIResponse;


namespace Project_MVC.Controllers
{
    public class AttendenceController : Controller
    {


        private readonly IHttpClientFactory _httpClientFactory;


        public AttendenceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;


        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new SettingDTO();


            // -----Url lay thonog tin department tu uid----------


            var apiUrl = "http://localhost:5260/api/Setting/GetSettingActive";

            // gọi api 
            var response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var departmentResponse = JsonSerializer.Deserialize<SettingDTO>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model = departmentResponse;

                return View(model);
            }
            else
            {
                return View(model);
            }


            //
            //if (setting == null) return Content("null");
            //return Content(setting.ClockInTime.ToString());
            // return content("null");
        }

        //clock in
        [HttpPost("checkIn")]
        public async Task<IActionResult> CheckIn()
        {

            // Tạo client và lấy token từ session
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }



            // Gọi API Clock In (API endpoint: POST /api/Attendance/ClockIn)
            var apiUrl = "http://localhost:5260/api/Attendences/ClockIn";
            // Gửi dữ liệu dưới dạng JSON
            var response = await client.PostAsJsonAsync(apiUrl, new { });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Check in thành công!";
                return RedirectToAction("Index", "Attendence"); // Chuyển hướng về trang chính chấm công
            }
            else
            {
                TempData["ErrorMessage"] = "Bạn đã check in ngày hôm nay rồi";
                return RedirectToAction("Index", "Attendence");
            }


        }


        //clock Out
        [HttpPost("checkOut")]
        public async Task<IActionResult> CheckOut()
        {
            // Tạo client và lấy token từ session
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }



            // Gọi API Clock In (API endpoint: POST /api/Attendance/ClockIn)
            var apiUrl = "http://localhost:5260/api/Attendences/ClockOut";
            // Gửi dữ liệu dưới dạng JSON
            var response = await client.PostAsJsonAsync(apiUrl, new { });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Check out thành công!";
                return RedirectToAction("Index", "Attendence"); // Chuyển hướng về trang chính chấm công
            }
            else
            {
                // Đọc nội dung lỗi từ API
                var errorContent = await response.Content.ReadAsStringAsync();

                // Phân tích nội dung lỗi (giả sử API trả về dạng JSON)
                try
                {

                    var errorObject = JsonSerializer.Deserialize<dynamic>(errorContent);
                    var errorMessage = errorObject?.title?.ToString() ?? errorObject?.ToString();

                    
                    // Xử lý các trường hợp lỗi cụ thể
                    if (errorMessage.Contains("Bạn chưa check in ngày hôm nay !!!"))
                    {
                        TempData["ErrorMessage"] = "Lỗi: " + errorMessage;
                    }
                    else if (errorMessage.Contains("Bạn đã check out ngày hôm nay rồi !!!"))
                    {
                        TempData["ErrorMessage"] = "Lỗi: " + errorMessage;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Lỗi không xác định: " + errorMessage;
                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = "Lỗi không xác định từ hệ thống";
                }

                return RedirectToAction("Index", "Attendence");
            }


        }








    }
}
