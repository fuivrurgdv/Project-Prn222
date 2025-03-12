using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.Department;
using Project_API.Data.Model;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public DepartmentController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }


        // lấy department theo id 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var departments = await _dbcontext.Departments.FirstOrDefaultAsync(x => x.DepartmentID == id);

            if (departments == null)
            {
                return BadRequest("khong thanh cong");
            }

            // Chuyển đổi sang DTO
            var departmentDTOs = new DepartmentDTO
            {
                DepartmentID = departments.DepartmentID,
                DepartmentName = departments.DepartmentName,
                Description = departments.Description,
                AdressDepartment = departments.AdressDepartment,
                IsActive = departments.IsActive,
            };

            return Ok(departmentDTOs);

        }


        // lấy các department
        [HttpGet("department")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _dbcontext.Departments.ToListAsync();

            // Chuyển đổi sang DTO
            var departmentDTOs = departments.Select(d => new DepartmentDTO
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName,
                Description = d.Description,
                AdressDepartment = d.AdressDepartment,
                IsActive = d.IsActive,
            }).ToList();

            return Ok(departmentDTOs);

        }

        // sửa  department theo id

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartments([FromRoute] int id,  DepartmentDTO dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != dto.DepartmentID)
            {
                return BadRequest("id không trùng khớp");
            }
            // tạo 1 department từ dto 
            var department = await _dbcontext.Departments.SingleOrDefaultAsync(x => x.DepartmentID == id);
            if (department == null)
            {
                return BadRequest("thong tin ko chinh xax");
            }


            department.DepartmentName = dto.DepartmentName;
            department.Description = dto.Description;
            department.AdressDepartment = dto.AdressDepartment;
            _dbcontext.Departments.Update(department);
            await _dbcontext.SaveChangesAsync();
            return Ok("thanh cong");

        }

        // Xóa department theo id 
        [HttpPut("disble/{id}")]
        public async Task<IActionResult> DisableDepartments(int id)
        {

            var department = await _dbcontext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            department.IsActive = false;
            _dbcontext.Departments.Update(department);
            await _dbcontext.SaveChangesAsync();
            return Ok("disable deparment thanh cong");


        }

        //kích hoạt department theo id
        [HttpPut("isAcive/{id}")]
        public async Task<IActionResult> IsActiveDepartments(int id)
        {

            var department = await _dbcontext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            department.IsActive = true;
            _dbcontext.Departments.Update(department);
            await _dbcontext.SaveChangesAsync();
            return Ok("iactive deparment thanh cong");


        }


        // Tạo mới department
        [HttpPost("department")]
        public async Task<IActionResult> CreaetDepartments(DepartmentRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = new Department
            {
                DepartmentName = request.DepartmentName,
                Description = request.Description,
                AdressDepartment = request.AdressDepartment,
                IsActive = true
            };

            _dbcontext.Departments.Add(department);
            await _dbcontext.SaveChangesAsync();

            return Ok("thêm department thành công");


        }


        //[HttpGet]
        //public string GetDepartmentNameById(int id)
        //{

        //}
    }
}
