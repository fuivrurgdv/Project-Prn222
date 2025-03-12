using Microsoft.AspNetCore.Mvc;
using Project_MVC.Models.Department;
using Project_MVC.Models.Users;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_MVC.Controllers
{
    public class DepartmentController : Controller
    {


        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersController _usersController;

        public DepartmentController(IHttpClientFactory httpClientFactory, UsersController usersController)
        {
            _httpClientFactory = httpClientFactory;
            _usersController = usersController;

        }






        // lấy các department
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new List<DepartmentDTO>();


            // -----Url lay thonog tin department tu uid----------


            var apiUrl = "http://localhost:5260/api/Department/department";

            // gọi api 
            var response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var departmentResponse = JsonSerializer.Deserialize<List<DepartmentDTO>>(json, new JsonSerializerOptions
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


        // trang ediit 
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
            var model = new DepartmentDTO();


            // -----Url lay thonog tin department tu uid----------


            var apiUrl = $"http://localhost:5260/api/Department/{id}";

            // gọi api 
            var response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var departmentResponse = JsonSerializer.Deserialize<DepartmentDTO>(json, new JsonSerializerOptions
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

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentDTO dto)
        {

            if (!ModelState.IsValid)
            {
                Console.WriteLine($"gia tri cua id , departmentid  la {id} ,{dto.DepartmentID}");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(dto);
            }
            if (id != dto.DepartmentID)
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
            var response = await client.PutAsync($"http://localhost:5260/api/Department/{dto.DepartmentID}", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật phòng ban thành công!";

                return RedirectToAction("GetDepartments");
            }
            else
            {

                TempData["ErrorMessage"] = "Cập nhật phòng ban thất bại!";
                return RedirectToAction("GetDepartments");
            }


        }



      
      




        [HttpGet]
        public IActionResult CreateDepartments()
        {
            return View(new DepartmentRequest());
        }

        //// Tạo mới department
        [HttpPost]
        public async Task<IActionResult> CreateDepartments(DepartmentRequest request)
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
            var response = await client.PostAsync("http://localhost:5260/api/Department/department", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm phòng ban thành công!";
                return RedirectToAction("GetDepartments");
            }
            TempData["ErrorMessage"] = "Thêm phòng ban thất bại!";
            return RedirectToAction("GetDepartments");


        }

        // kích hoạt vphòng ban
        [HttpPut]
        public async Task<IActionResult> IActiveDepartment(int id)
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
            var apiUrl = $"http://localhost:5260/api/Department/isAcive/{id}";



            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {


                return Content("is active thanh cong");



            }
            else
            {
                return Content(apiUrl);
            }


        }

        // khóa phòng ban
        [HttpPut]
        public async Task<IActionResult> DisabeDepartment(int id)
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
            var apiUrl = $"http://localhost:5260/api/Department/disble/{id}";



            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {

                return Content("disable thanh cong");




            }
            else
            {
                return Content(apiUrl);
            }

        }


    }
}
