using Microsoft.AspNetCore.Mvc;
using Project_MVC.Models.SalaryLevel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Project_MVC.Controllers
{
    public class SalaryLevelController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public SalaryLevelController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }


        // lay danh  sach position tu 1 id  department
        [HttpGet]
        public async Task<List<SalaryLevelDTO>> GetSalaryLevel()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new List<SalaryLevelDTO>();


            // -----Url lay thonog tin nhan vien tu uid----------
            var apiUrl = $"http://localhost:5260/api/Salary";




            var response = await client.GetAsync(apiUrl);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {
                //chuỗi kết quả nhận được từ API
                var json = await response.Content.ReadAsStringAsync();
                // Convert chuỗi sang model 
                var pageResponse = JsonSerializer.Deserialize<List<SalaryLevelDTO>>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model = pageResponse;

                return model;




            }
            else
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetSalaryLevelView()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new List<SalaryLevelDTO>();


            // -----Url lay thonog tin nhan vien tu uid----------
            var apiUrl = $"http://localhost:5260/api/Salary";




            var response = await client.GetAsync(apiUrl);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {
                //chuỗi kết quả nhận được từ API
                var json = await response.Content.ReadAsStringAsync();
                // Convert chuỗi sang model 
                var pageResponse = JsonSerializer.Deserialize<List<SalaryLevelDTO>>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model = pageResponse;

                return View("Create",model);




            }
            else
            {
                return null;
            }

        }

        public IActionResult Index()
        {
            return View();
        }

        // tao salary moi 

        [HttpPost]
        public async Task<IActionResult> CreateSalaryLevel(SalaryRequest request)
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
            var response = await client.PostAsync("http://localhost:5260/api/Salary", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage123"] = "Thêm lương cơ bản thành công!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage123"] = "Thêm lương cơ bản thất bại!";
            return RedirectToAction("Index");


        }

        [HttpGet]
        public IActionResult CreateSalaryLevel()
        {
            return View(new SalaryRequest());
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, SalaryLevelDTO dto)
        {

            if (!ModelState.IsValid)
            {

                return View(dto);
            }
            if (id != dto.SalaryLevelId)
            {
                return View(dto);
            }
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"http://localhost:5260/api/Salary/{id}", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage123"] = "Cập nhật mức lương thành công!";

                return RedirectToAction("Index");
            }
            else
            {

                TempData["ErrorMessage123"] = "Cập nhật mức lương thất bại!";
                return RedirectToAction("Index");
            }


        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new SalaryLevelDTO();


            // -----Url lay thonog tin department tu uid----------


            var apiUrl = $"http://localhost:5260/api/Salary/{id}";

            // gọi api 
            var response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var departmentResponse = JsonSerializer.Deserialize<SalaryLevelDTO>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model = departmentResponse;
                return View(model);

            }
            else
            {
                return Content(apiUrl);
            }



        }

    }
}
