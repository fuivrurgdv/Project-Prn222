using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.LeaveRequest;
using Project_API.Data.Model;
using System.Security.Claims;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        
        public LeaveRequestController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
            
        }

        [HttpGet("leaveRequest")]
        public async Task<IActionResult> GetLeaveRequest() {
            var leaveRequests = await _dbcontext.LeaveRequests.ToListAsync();

            // Chuyển đổi sang DTO
            var leaveRequestDTOs = leaveRequests.Select(d => new LeaveRequestDTO
            {
                RequestID = d.RequestID,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                UserID = d.UserID,
                status = d.status,
                Reason = d.Reason,
            }).ToList();

            return Ok(leaveRequestDTOs);
        }

        [HttpPost("SendLeaveRequest")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendLeaveRequest(SendLeaveRequest request)
        {
            // Lấy userID từ token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Token không có claim NameIdentifier");
            }
            int userId = int.Parse(userIdClaim.Value);
            var user = _dbcontext.Users.FirstOrDefault(e => e.UserID == userId);

            // Kiểm tra loại nghỉ hợp lệ
            /*if (string.IsNullOrEmpty(request.LeaveType) ||
                (request.LeaveType != "morning" && request.LeaveType != "afternoon" && request.LeaveType != "multiple_days"))
            {
                return BadRequest("Loại nghỉ không hợp lệ.");
            }

            // Bắt buộc có StartDate
            if (!request.StartDate.HasValue)
            {
                return BadRequest("Ngày bắt đầu nghỉ là bắt buộc.");
            }

            // Kiểm tra logic theo loại nghỉ
            if (request.LeaveType == "multiple_days")
            {
                if (!request.EndDate.HasValue)
                {
                    return BadRequest("Nghỉ nhiều ngày phải có ngày kết thúc.");
                }

                if (request.EndDate < request.StartDate)
                {
                    return BadRequest("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
                }
            }
            else // Nghỉ buổi sáng hoặc buổi chiều
            {
                if (request.EndDate.HasValue)
                {
                    return BadRequest("Không cần ngày kết thúc khi nghỉ buổi sáng hoặc buổi chiều.");
                }
            }*/

            // Chuyển đổi sang DTO

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

*/
            var leaveRequests = new LeaveRequests
            {
                
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                LeaveType = request.LeaveType,
                UserID = userId,
                Reason = request.Reason,
                status = 0,
            };

            _dbcontext.LeaveRequests.Add(leaveRequests);
            await _dbcontext.SaveChangesAsync();

            return Ok("đã nộp đơn thành công");


            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, LeaveRequestDTO request)
        {
            var leaveRequest = await _dbcontext.LeaveRequests.SingleOrDefaultAsync(x => x.RequestID == id);
            if (leaveRequest == null)
            {
                return NotFound("Không tìm thấy đơn nghỉ. quang");
            }

            leaveRequest.status = request.status;
            _dbcontext.LeaveRequests.Update(leaveRequest);
            await _dbcontext.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công!" });
        }



    }
}
