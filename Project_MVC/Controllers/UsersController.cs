using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Project_MVC.Models.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;
using System.Text.Json;
using Project_MVC.Models.Department;
using Project_MVC.Models.Position;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Project_MVC.Models.History;
using System.Net.Http;
using System.Text;
using System.Reflection;
using Project_MVC.Models.SalaryLevel;

namespace Project_MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PositionController _positionController;
        private readonly SalaryLevelController _salaryLevelController;

        public UsersController(SalaryLevelController salaryLevelController, IHttpClientFactory httpClientFactory, PositionController position)
        {
            _httpClientFactory = httpClientFactory;
            _positionController = position;
            _salaryLevelController = salaryLevelController;

        }

        //danh sách nhân veien
        [HttpGet]
        public async Task<IActionResult> Index([Bind(Prefix = "UserQueryParameters")] UserQueryParameters param)
        {
            // tạo 1 httpclient để guiwr request tới api 
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new PageResponse();
            model.UserQueryParameters = param;

            // -----Url lay danh sach phan trang nhan vien----------
            var apiUrl = "http://localhost:5260/api/User";
            var queryParams = $"?CurrentPage={param.CurrentPage}&PageSize={param.PageSize}";

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                queryParams += $"&Search={param.Search}";
            }
            if (!string.IsNullOrWhiteSpace(param.Gender))
            {
                queryParams += $"&Gender={param.Gender}";
            }

            if (param.DepartmentId.HasValue)
            {
                queryParams += $"&DepartmentId={param.DepartmentId}";



                //---- end call api position ----



            }

            if (param.PositionId.HasValue)
            {
                queryParams += $"&PositionId={param.PositionId}";

            }

            //call api method = get
            var response = await client.GetAsync(apiUrl + queryParams);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {
                //chuỗi kết quả nhận được từ API
                var json = await response.Content.ReadAsStringAsync();
                // Convert chuỗi sang model 
                var pageResponse = JsonSerializer.Deserialize<UserResponseQuery>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });
                model.UserResponseQuery = pageResponse;

                //return View(model);




            }
            else
            {
                return Content(apiUrl + queryParams);
            }


            // -----------Lấy danh sách các department ---------------
            apiUrl = "http://localhost:5260/api/Department/department";

            // gọi api 
            response = await client.GetAsync(apiUrl);

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

                model.ListDepartment = departmentResponse;
                //string s = "";
                //foreach (var x in model.ListDepartment)
                //{
                //    s += x.DepartmentName + " ,";
                //}
                //return Content(s);
            }
            else
            {
                return Content(apiUrl + queryParams);
            }


            // -------------------Lấy danh sách Position theo department----------------------------------------
            if (param.DepartmentId.HasValue)
            {
                var apiUrlPositions = $"http://localhost:5260/api/Position/department/{param.DepartmentId.Value}";
                var responsePositions = await client.GetAsync(apiUrlPositions);
                if (responsePositions.IsSuccessStatusCode)
                {
                    var json = await responsePositions.Content.ReadAsStringAsync();
                    var positionResponse = JsonSerializer.Deserialize<List<PositionDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    model.ListPosition = positionResponse;
                }

                else
                {
                    return Content(apiUrlPositions);
                }
            }
            else
            {
                model.ListPosition = new List<PositionDTO>();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ViewPartial", model);

            }
            else return View(model);
        }


        //thông tin nhân viên theo id
        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------
            var model = new UserDTOView();


            // -----Url lay thonog tin nhan vien tu uid----------
            var apiUrl = $"http://localhost:5260/api/User/user/{id}";



            var response = await client.GetAsync(apiUrl);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {
                //chuỗi kết quả nhận được từ API
                var json = await response.Content.ReadAsStringAsync();
                // Convert chuỗi sang model 
                var pageResponse = JsonSerializer.Deserialize<UserDTO>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model.UserDTO = pageResponse;

                // return View("InformationUser", pageResponse);




            }
            else
            {
                return Content(apiUrl);
            }

            apiUrl = $" http://localhost:5260/api/User/{id}/work-history";



            response = await client.GetAsync(apiUrl);

            // nếu mà thành công
            if (response.IsSuccessStatusCode)
            {
                //chuỗi kết quả nhận được từ API
                var json = await response.Content.ReadAsStringAsync();
                // Convert chuỗi sang model 
                var pageResponse = JsonSerializer.Deserialize<List<HistoryWork>>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model.HistoryWork = pageResponse;



            }
            else
            {
                return Content(apiUrl);
            }

            return View("InformationUser", model);

        }


        // xóa nhân viên
        [HttpPut]
        public async Task<IActionResult> IActiveUser(int id)
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
            var apiUrl = $"http://localhost:5260/api/User/isActive/{id}";



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

        // xóa nhân viên
        [HttpPut]
        public async Task<IActionResult> DisabeUser(int id)
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
            var apiUrl = $"http://localhost:5260/api/User/disable/{id}";



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

        //thêm nhân viên
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            var client = _httpClientFactory.CreateClient();

            // lấy token  từ sesion và gán vào header, 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            //----Tạo model để trả về cho view-------

            var model = new AddUserRequestPage();

            // -----------Lấy danh sách các department ---------------
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

                model.ListDepartment = departmentResponse;
                //return Content(model.ListDepartment.Count().ToString()); 
            }
            else
            {
                model.ListDepartment = new List<DepartmentDTO>();
            }
            // -----------Lấy danh sách salaryLevel ---------------
            apiUrl = "http://localhost:5260/api/Salary";

            // gọi api 
            response = await client.GetAsync(apiUrl);

            //nếu thành công
            if (response.IsSuccessStatusCode)
            {
                //kết quả dưới dạng json
                var json = await response.Content.ReadAsStringAsync();
                // chuyển từ json sang department list
                var saalryResponse = JsonSerializer.Deserialize<List<SalaryLevelDTO>>(json, new JsonSerializerOptions
                {
                    // không phân biệt chữ hoa chữ thường khi convert từ json sang
                    PropertyNameCaseInsensitive = true
                });

                model.ListSalaryLevelName = saalryResponse;
                //return Content(model.ListDepartment.Count().ToString()); 
            }
            else
            {
                model.ListDepartment = new List<DepartmentDTO>();
            }
            model.ListPosition = new List<PositionDTO>();
            return View(model);


        }


        [HttpPost]
        public async Task<IActionResult> AddUser([Bind(Prefix = "UserRequest")] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                //string debugInfo = $"DepartmentId: {request.DepartmentId}, PositionId: {request.PositionId}, SalaryLevelId: {request.SalaryLevelId}";

                //// Lấy danh sách lỗi từ ModelState
                //var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                //string errorMessages = string.Join("; ", errors);

                //// Trả về nội dung để debug
                //return Content($"Debug Info: {debugInfo} | ModelState Errors: {errorMessages}");


                //string s = "";
                //s += request.DepartmentId + "," + request.PositionId + "," + request.SalaryLevelId;
                //return Content(ModelS);
                var model = new AddUserRequestPage
                {
                    UserRequest = request
                };

                return View(model);
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
            var response = await client.PostAsync("http://localhost:5260/api/User", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return Content("thêm thất bại");
            }

        }

    }
}
