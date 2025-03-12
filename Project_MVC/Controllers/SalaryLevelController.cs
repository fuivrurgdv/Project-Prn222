using Microsoft.AspNetCore.Mvc;
using Project_MVC.Models.SalaryLevel;
using System.Net.Http.Headers;
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
