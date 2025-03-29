using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.SalaryLevel;
using Project_API.Data.Model;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public SalaryController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }

        //lấy danh sách các salarylevel
        [HttpGet]
        public async Task<IActionResult> GetSalaryLevel()
        {
            var list = await _dbcontext.SalaryLevels.ToListAsync();
            var listDTO = list.Select(x => new SalaryLevelDTO
            {
                SalaryLevelId = x.SalaryLevelId,
                LevelName = x.LevelName,
                BasicSalary = x.BasicSalary,
                Description = x.Description
            });
            return Ok(listDTO);
        }

        // Thêm salaryLevel
        [HttpPost]
        public async Task<IActionResult> AddSalaryLevel(SalaryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("khong thanh cong");
            }
            var salaryLevel = new SalaryLevel()
            {
                BasicSalary = request.BasicSalary,
                Description = request.Description,
                LevelName = request.LevelName,


            };
            _dbcontext.SalaryLevels.Add(salaryLevel);
            await _dbcontext.SaveChangesAsync();
            return Ok("thanh cong");
        }


        // sửa salarylevel
        [HttpPut("{id}")]
        public async Task<IActionResult> EditSalaryLevel([FromRoute] int id, SalaryLevelDTO request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("requets khong thanh cong");

            }
            if (id != request.SalaryLevelId)
            {
                return BadRequest("id không trùng khớp");
            }
            var x = await _dbcontext.SalaryLevels.FirstOrDefaultAsync(x => x.SalaryLevelId == id);
            if (x == null)
            {
                return BadRequest("bi loi roi");
            }
            x.LevelName = request.LevelName;
            x.Description = request.Description;
            x.BasicSalary = request.BasicSalary;
            _dbcontext.SalaryLevels.Update(x);
            await _dbcontext.SaveChangesAsync();
            return Ok("thanh cong");
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalaryLevelById(int id)
        {
            var list = await _dbcontext.SalaryLevels.FirstOrDefaultAsync(x => x.SalaryLevelId == id);
            var dto = new SalaryLevelDTO()
            {
                SalaryLevelId = id,
                BasicSalary = list.BasicSalary,
                Description = list.Description,
                LevelName = list.LevelName,

            };
            return Ok(dto);
        }

        //tính lương của nhân viên hằng tháng 

        //danh sách trả lương cho nhân viên theo tháng theo năm 

    }
}
