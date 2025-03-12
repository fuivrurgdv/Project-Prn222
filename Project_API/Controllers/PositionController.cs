using Azure.Core;
using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.Position;
using Project_API.Data.Model;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public PositionController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }

        // lay danh  sach position tu 1 id  department
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetPositionsByDepartment(int departmentId)
        {
            var positions = await _dbcontext.Positions
                .Where(p => p.DepartmentID == departmentId)
                .ToListAsync();

            // Nếu sử dụng DTO:
            var positionDTOs = positions.Select(p => new PositionDTO
            {
                PositionID = p.PositionID,
                PositionName = p.PositionName,
                Description = p.Description,
                IsActive = p.IsActive,
                PositionAllowance = p.PositionAllowance
            });

            return Ok(positionDTOs);
        }


        // them position 
        [HttpPost]
        public async Task<IActionResult> CreatePosition([FromBody] PositionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var position = new Position
            {
                PositionName = request.PositionName,
                Description = request.Description,

                IsActive = true, // Khi tạo mới, mặc định là active
                PositionAllowance = request.PositionAllowance
            };
            var deparment = await _dbcontext.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName == request.DepartmentName);
            if (deparment == null) return BadRequest("khong thanh cong !!");
            position.DepartmentID = deparment.DepartmentID;
            _dbcontext.Positions.Add(position);
            await _dbcontext.SaveChangesAsync();



            return Ok("them position thanh cong");
        }


        // sua position
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] PositionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.PositionID)
            {
                return BadRequest("id khong khop");
            }


            var existName = await _dbcontext.Positions
                .FirstOrDefaultAsync(x => x.PositionID != id && x.PositionName == dto.PositionName);
            if (existName != null)
            {
                return BadRequest("name postion da ton tai");
            }

            var position = await _dbcontext.Positions
                .FirstOrDefaultAsync(x => x.PositionID == id);
            if (position != null)
            {
                position.PositionName = dto.PositionName;
                position.Description = dto.Description;

                position.IsActive = dto.IsActive;
                position.PositionAllowance = dto.PositionAllowance;



                _dbcontext.Positions.Update(position);
                await _dbcontext.SaveChangesAsync();
                return Ok("sua position thanh cong");
            }

            return BadRequest("khong thanh cong");




        }


        //// vo hieu hoa position

        //[HttpPatch("disable/{id}")]
        //public async Task<IActionResult> DisablePosition(int id)
        //{

        //    var position = await _dbcontext.Positions
        //         .FirstOrDefaultAsync(x => x.PositionID == id);
        //    if (position != null)
        //    {
        //        position.IsActive = false;
        //        _dbcontext.Positions.Update(position);
        //        await _dbcontext.SaveChangesAsync();
        //        return Ok("vo hieu hoa position thanh cong");
        //    }

        //    return BadRequest("khong thanh cong");





        //}


        //// kich hoat position
        //[HttpPatch("isActive/{id}")]
        //public async Task<IActionResult> IactivePosition(int id)
        //{

        //    var position = await _dbcontext.Positions
        //       .FirstOrDefaultAsync(x => x.PositionID == id);
        //    if (position != null)
        //    {
        //        position.IsActive = true;
        //        _dbcontext.Positions.Update(position);
        //        await _dbcontext.SaveChangesAsync();
        //        return Ok("active position thanh cong");
        //    }

        //    return BadRequest("khong thanh cong");
        //}


    }
}
