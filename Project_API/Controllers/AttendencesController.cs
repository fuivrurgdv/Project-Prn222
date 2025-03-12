using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.APIResponse;
using Project_API.Data.DTO.Attendance;
using Project_API.Data.Model;
using System.Security.Claims;

namespace Project_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendencesController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public AttendencesController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }


        // chấm công lúc vao
        [HttpPost("ClockIn")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ClockIn()
        {
            // Lấy userID từ token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Token không có claim NameIdentifier");
            }
            int userId = int.Parse(userIdClaim.Value);

            // ... Sử dụng userId để tạo record chấm công ...
            // var attendance = new Attendance { UserID = userId, ... };




            // Lấy ca làm việc hiện hành 
            var shiftSetting = await _dbcontext.ShiftSettings.FirstOrDefaultAsync(x => x.IsActive == true);
            if (shiftSetting == null)
            {
                return BadRequest("Chưa có cấu hình ca làm việc.");
            }

            // Xác định ScheduledClockIn và ScheduledClockOut cho ngày làm việc hiện tại

            DateTime today = DateTime.Now.Date;
            DateTime ScheduledClockIn = today + shiftSetting.ClockInTime.TimeOfDay;
            DateTime ScheduledClockOut = today + shiftSetting.ClockOutTime.TimeOfDay;
            // Kiểm tra nếu đã Clock In
            var existing = await _dbcontext.Attendances
                .FirstOrDefaultAsync(a => a.UserID == userId && a.WorkDate.Date == today);
            if (existing != null)
            {
                return BadRequest("Bạn đã Clock In cho ngày này.");
            }


            DateTime actualClockIn = DateTime.Now;


            var attendance = new Attendance
            {
                UserID = userId,
                WorkDate = today,
                ClockIn = actualClockIn,
                ScheduledClockIn = ScheduledClockIn,
                ScheduledClockOut = ScheduledClockOut,

            };

            _dbcontext.Attendances.Add(attendance);
            await _dbcontext.SaveChangesAsync();
            return Ok("Clock In thành công!");
        }

        //chấm công lúc ra
        [HttpPost("ClockOut")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ClockOut()
        {

            // Lấy userID từ token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Token không có claim NameIdentifier");
            }
            int userId = int.Parse(userIdClaim.Value);


            DateTime today = DateTime.Now.Date;
            var attendance = await _dbcontext.Attendances.FirstOrDefaultAsync(a =>
                                a.UserID == userId &&
                                a.WorkDate.Date == today);
            if (attendance == null)
            {
                return NotFound(new
                {
                    title = "Lỗi nghiệp vụ",
                    status = 404,
                    detail = "Bạn chưa check in ngày hôm nay !!!"
                });
            }
            if (attendance.ClockOut != null)
            {
                return NotFound(new
                {
                    title = "Lỗi nghiệp vụ",
                    status = 404,
                    detail = "Bạn đã check out ngày hôm nay rồi !!!"
                });
            }


            attendance.ClockOut = DateTime.Now;


            attendance.ToTalHours = (attendance.ClockOut.Value - attendance.ClockIn).Value.TotalHours;

            _dbcontext.Attendances.Update(attendance);
            await _dbcontext.SaveChangesAsync();

            return Ok("Clock Out thành công!");
        }

        ////lịch sử chấm công

        //[HttpGet]
        //[Authorize(Roles = "User")]
        //public async Task<IActionResult> GetHistoryAttendencesOneUser(UserAttendenceQueryParameters request)
        //{
        //    // Lấy userID từ token
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized("Token không có claim NameIdentifier");
        //    }
        //    int userId = int.Parse(userIdClaim.Value);


            
        //    var query =  _dbcontext.Attendances.AsQueryable().Where(a => a.UserID == userId);


           

        //    if (request.DateAttendence.HasValue)
        //    {
        //        query = query.Where(a => a.WorkDate.Date >= request.DateAttendence.Value.Date);
        //    }
        //    if (request.HasValue)
        //    {
        //        query = query.Where(a => a.WorkDate.Date <= endDate.Value.Date);
        //    }

        //    int totalCount = await query.CountAsync();
        //    int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        //    var data = await query
        //        .OrderByDescending(a => a.WorkDate)
        //        .Skip((currentPage - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    // Đóng gói kết quả theo cấu trúc phân trang
        //    var result = new
        //    {
        //        currentPage,
        //        pageSize,
        //        totalCount,
        //        totalPages,
        //        data
        //    };

        //    return Ok(result);


        //    if (attendance == null)
        //    {
        //        return NotFound(new
        //        {
        //            title = "Lỗi nghiệp vụ",
        //            status = 404,
        //            detail = "Bạn chưa check in ngày hôm nay !!!"
        //        });
        //    }
        //    if (attendance.ClockOut != null)
        //    {
        //        return NotFound(new
        //        {
        //            title = "Lỗi nghiệp vụ",
        //            status = 404,
        //            detail = "Bạn đã check out ngày hôm nay rồi !!!"
        //        });
        //    }


        //    attendance.ClockOut = DateTime.Now;


        //    attendance.ToTalHours = (attendance.ClockOut.Value - attendance.ClockIn).Value.TotalHours;

        //    _dbcontext.Attendances.Update(attendance);
        //    await _dbcontext.SaveChangesAsync();

        //    return Ok("Clock Out thành công!");
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetListHistoryAttendencesEmployee()
        //{
        //    // Lấy userID từ token
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized("Token không có claim NameIdentifier");
        //    }
        //    int userId = int.Parse(userIdClaim.Value);


        //    DateTime today = DateTime.Now.Date;
        //    var attendance = await _dbcontext.Attendances.FirstOrDefaultAsync(a =>
        //                        a.UserID == userId &&
        //                        a.WorkDate.Date == today);
        //    if (attendance == null)
        //    {
        //        return NotFound(new
        //        {
        //            title = "Lỗi nghiệp vụ",
        //            status = 404,
        //            detail = "Bạn chưa check in ngày hôm nay !!!"
        //        });
        //    }
        //    if (attendance.ClockOut != null)
        //    {
        //        return NotFound(new
        //        {
        //            title = "Lỗi nghiệp vụ",
        //            status = 404,
        //            detail = "Bạn đã check out ngày hôm nay rồi !!!"
        //        });
        //    }


        //    attendance.ClockOut = DateTime.Now;


        //    attendance.ToTalHours = (attendance.ClockOut.Value - attendance.ClockIn).Value.TotalHours;

        //    _dbcontext.Attendances.Update(attendance);
        //    await _dbcontext.SaveChangesAsync();

        //    return Ok("Clock Out thành công!");
        //}

        //[HttpGet("user/{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetListHistoryAttendencesEmployee(int id)
        //{
        //    // Lấy userID từ token
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized("Token không có claim NameIdentifier");
        //    }
        //    int userId = int.Parse(userIdClaim.Value);


        //    DateTime today = DateTime.Now.Date;
        //    var attendance = await _dbcontext.Attendances.FirstOrDefaultAsync(a =>
        //                        a.UserID == userId &&
        //                        a.WorkDate.Date == today);
        //    if (attendance == null)
        //    {
        //        return NotFound(new
        //        {
        //            title = "Lỗi nghiệp vụ",
        //            status = 404,
        //            detail = "Bạn chưa check in ngày hôm nay !!!"
        //        });
        //    }
        //    if (attendance.ClockOut != null)
        //    {
        //        return NotFound(new
        //        {
        //            title = "Lỗi nghiệp vụ",
        //            status = 404,
        //            detail = "Bạn đã check out ngày hôm nay rồi !!!"
        //        });
        //    }


        //    attendance.ClockOut = DateTime.Now;


        //    attendance.ToTalHours = (attendance.ClockOut.Value - attendance.ClockIn).Value.TotalHours;

        //    _dbcontext.Attendances.Update(attendance);
        //    await _dbcontext.SaveChangesAsync();

        //    return Ok("Clock Out thành công!");
        //}

    }
}
