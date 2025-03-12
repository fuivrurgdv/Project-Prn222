using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.Setting;
using Project_API.Data.Model;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public SettingController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }

        //lấy setting is active
        [HttpGet("GetSettingActive")]
        public async Task<IActionResult> GetSettingIsActive()
        {
            var setting = await _dbcontext.ShiftSettings.FirstOrDefaultAsync(x => x.IsActive == true);
            if (setting == null)
            {
                return BadRequest("khong thanh cong");
            }
            var settingDTO = new SettingDTO()
            {
                ShiftSettingID = setting.ShiftSettingID,
                ClockInTime = setting.ClockInTime,
                IsActive = setting.IsActive,
                ClockOutTime = setting.ClockOutTime,
                CreateDay = setting.CreateDay,

            };
            return Ok(settingDTO);
        }


        //lấy danh sách các settingcontroller
        [HttpGet("GếtttingList")]
        public async Task<IActionResult> GetSetting()
        {
            var list = await _dbcontext.ShiftSettings.ToListAsync();
            var listDTO = list.Select(x => new SettingDTO
            {
                ShiftSettingID = x.ShiftSettingID,
                ClockInTime = x.ClockInTime,
                ClockOutTime = x.ClockOutTime,
                CreateDay = x.CreateDay,
                IsActive = x.IsActive,



            });
            return Ok(listDTO);
        }

        // Thêm 1 settingControoler
        [HttpPost("Add")]
        public async Task<IActionResult> CreateSetting(SettingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("them khongg thanh cong");
            }
            var setting = new ShiftSetting()
            {
                ClockInTime = request.ClockInTime,
                ClockOutTime = request.ClockOutTime,
                CreateDay = DateTime.Now,
                IsActive = false,
            };
            _dbcontext.ShiftSettings.Add(setting);
            await _dbcontext.SaveChangesAsync();
            return Ok("Them thanh cong setting");
        }

        // active setting
        [HttpPut("isActive/{id}")]
        public async Task<IActionResult> ActiveSetting(int id)
        {
            var setting = await _dbcontext.ShiftSettings.FirstOrDefaultAsync(x => x.ShiftSettingID == id);

            if (setting == null)
            {
                return BadRequest("khong thanh cong");
            }

            setting.IsActive = true;
            _dbcontext.ShiftSettings.Update(setting);
            await _dbcontext.SaveChangesAsync();
            return Ok("active thanh cong setting");
        }

        // disabel setting
        [HttpPut("disable/{id}")]
        public async Task<IActionResult> DisableSetting(int id)
        {
            var setting = await _dbcontext.ShiftSettings.FirstOrDefaultAsync(x => x.ShiftSettingID == id);

            if (setting == null)
            {
                return BadRequest("khong thanh cong");
            }

            setting.IsActive = false;
            _dbcontext.ShiftSettings.Update(setting);
            await _dbcontext.SaveChangesAsync();
            return Ok("disable thanh cong setting");
        }
    }
}
