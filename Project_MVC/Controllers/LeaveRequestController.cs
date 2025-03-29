using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Project_MVC.Models.LeaveRequest;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Project_MVC.Controllers
{
    public class LeaveRequestController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersController _usersController;

        public LeaveRequestController(IHttpClientFactory httpClientFactory, UsersController usersController)
        {
            _httpClientFactory = httpClientFactory;
            _usersController = usersController;

        }
        [HttpGet]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new List<LeaveRequestDTO>();


            // -----Url lay thonog tin department tu uid----------


            var apiUrl = "http://localhost:5260/api/LeaveRequest/leaveRequest";

            // gọi api 
            var response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var LeaveRequestResponse = JsonSerializer.Deserialize<List<LeaveRequestDTO>>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model = LeaveRequestResponse;
                return View("Index", model);

            }
            else
            {
                return Content(apiUrl);
            }

        }

        [HttpGet]
        public IActionResult SendLeaveRequest()
        {
            return View("Send", new SendLeaveRequest());
        }

        //// Tạo mới department
        [HttpPost]
        public async Task<IActionResult> SendLeaveRequest(SendLeaveRequest request)
        {

            if (request.LeaveType == "multiple_days" && (request.StartDate == null || request.EndDate == null))
            {
                ModelState.AddModelError("StartDate", "Vui lòng nhập ngày bắt đầu.");
                ModelState.AddModelError("EndDate", "Vui lòng nhập ngày kết thúc.");
            }

            if (request.LeaveType == "morning" || request.LeaveType == "afternoon")
            {
                request.StartDate = DateTime.Now; // Gán ngày hiện tại
                request.EndDate = DateTime.Now; // Không cần ngày kết thúc
            }

            if (!ModelState.IsValid)
            {
                return View("Send", request);
            }

            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            Console.WriteLine($"Nội dung gửi API: {JsonSerializer.Serialize(request)}");
            // Gửi yêu cầu POST đến API CreateUser
            var response = await client.PostAsync("http://localhost:5260/api/LeaveRequest/SendLeaveRequest", jsonContent);
            Console.WriteLine($"Mã phản hồi từ API: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Nội dung phản hồi từ API: {responseContent}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Gửi đơn thành công!";
                return RedirectToAction("SendLeaveRequest");
            }
            TempData["ErrorMessage"] = "Gửi đơn thất bại!";
            return View("Send", request);


        }

        [HttpPost]
        public IActionResult TestUpdateStatus([FromRoute] int id)
        {
            Console.WriteLine("TestUpdateStatus với id: " + id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, LeaveRequestDTO request)
        {
            if (request.status != 1 && request.status != 2)
            {
                return BadRequest("Trạng thái không hợp lệ.");
            }
            Console.WriteLine($"gia tri cua id , LeaveRequestId  la {id} ,{request.RequestID}");
            if (id != request.RequestID)
            {
                TempData["ErrorMessage"] = "RequestID không khớp.";
                return RedirectToAction("GetLeaveRequests");
            }

            var client = _httpClientFactory.CreateClient();

            // Lấy token từ session và gán vào header
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Tạo nội dung JSON để gửi API
            
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            Console.WriteLine($"Nội dung gửi API: {JsonSerializer.Serialize(request)}");
            // Gọi API cập nhật status
            var response = await client.PutAsync($"http://localhost:5260/api/LeaveRequest/{request.RequestID}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
                return RedirectToAction("GetLeaveRequests"); // Quay lại danh sách
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync(); // Đọc nội dung lỗi
                TempData["ErrorMessage"] = $"Cập nhật trạng thái thất bại! Lỗi: {response.StatusCode} - {errorContent}";
                return RedirectToAction("GetLeaveRequests"); // Quay lại danh sách
            }

            
        }

    }
}
