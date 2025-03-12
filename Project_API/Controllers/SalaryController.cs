using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.SalaryLevel;

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
    }
}
