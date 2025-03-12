using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.HistoryWork;
using Project_API.Data.DTO.Users;
using Project_API.Data.Model;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        private readonly RoleController _roleController;
        public UserController(IConfiguration configuration, MyDBContext dbcontext, RoleController roleController)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
            _roleController = roleController;
        }

        // thêm nhân viên  , tự động là user
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRequest request)
        {
            //chua validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //tạo 1 user và convert từ request sang user
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
                Gender = request.Gender,
                Username = request.Username,
                Password = request.Password,
                DateJoined = DateTime.Now,
                Phone = request.Phone,
                IsActive = true
            };

            // tìm slarylevelid từ salaryname và gán cho user.salaryid
            var salary = await _dbcontext.SalaryLevels.FirstOrDefaultAsync(x => x.SalaryLevelId == request.SalaryLevelId);
            if (salary != null)
            {
                user.SalaryLevelId = salary.SalaryLevelId;
            }
            _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync();

            // tìm departmentID và positionID , sau đó nhét vào bảng employeeDepartmenHstry
            var department = await _dbcontext.Departments
                .FirstOrDefaultAsync(x => x.DepartmentID == request.DepartmentId);
            var position = await _dbcontext.Positions
               .FirstOrDefaultAsync(x => x.PositionID == request.PositionId);
            if (position != null && department != null)
            {
                var history = new EmployeeDepartmentHistory()
                {
                    UserID = user.UserID,
                    DepartmentID = department.DepartmentID,
                    PositionID = position.PositionID,
                    StartDate = DateTime.Now,
                    EndDate = null,
                };
                _dbcontext.EmployeeDepartmentHistories.Add(history);
                await _dbcontext.SaveChangesAsync();



            }

            // thêm role cho user , tự động là User
            var role = await _dbcontext.Roles.SingleOrDefaultAsync(x => x.RoleName == "User");
            if (role != null)
            {

                var userrole = new UserRole()
                {
                    RoleID = role.RoleID,
                    UserID = user.UserID
                };
                _dbcontext.UserRoles.Add(userrole);
                await _dbcontext.SaveChangesAsync();
            }

            return Ok("da them user thanh cong");

            



        }

        //thông tin nhân viên theo id
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {

            // check id
            var user = await _dbcontext.Users.SingleOrDefaultAsync(x => x.UserID == id);
            if (user == null) return BadRequest("khong tim dc id");




            //tạo userdto và convert tu user sang DTO
            var userDTO = new UserDTO()
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                Gender = user.Gender,
                Phone = user.Phone,
                DateJoined = user.DateJoined,
                IsActive = user.IsActive,

            };
            userDTO.DepartmentName = _dbcontext.EmployeeDepartmentHistories
              .Include(x => x.Department)
             .Where(a => a.UserID == user.UserID && a.EndDate == null)
             .Select(z => z.Department.DepartmentName).FirstOrDefault();

            userDTO.PositionName = _dbcontext.EmployeeDepartmentHistories
            .Include(x => x.Position)
           .Where(a => a.UserID == user.UserID && a.EndDate == null)
           .Select(z => z.Position.PositionName).FirstOrDefault();

            userDTO.SalaryLevelName = _dbcontext.SalaryLevels
            .FirstOrDefault(a => a.SalaryLevelId == user.SalaryLevelId).LevelName;


            userDTO.RoleName = _roleController.GetRoleUser(user) ?? null;



            return Ok(userDTO);


        }



        // quá trình công tác của 1 nhân viên

        [HttpGet("{id}/work-history")]
        public async Task<IActionResult> GetWorkHistory(int id)
        {
            var history = await _dbcontext.EmployeeDepartmentHistories
            .Where(h => h.UserID == id)
            .OrderByDescending(h => h.StartDate)
            .ToListAsync();
            var historyDTO = history.Select(x => new HistoryWork()
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DepartmentName = _dbcontext.Departments
                .FirstOrDefault(x => x.DepartmentID == x.DepartmentID).DepartmentName,
                PositionName = _dbcontext.Positions
                .FirstOrDefault(x => x.PositionID == x.PositionID).PositionName
            });
            return Ok(historyDTO);
        }


        //Danh sách nhân viên với tìm kiếm
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] UserQueryParameters queryParams)

        {
            var query = _dbcontext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                query = query.Where(e => e.FirstName.ToLower().Contains(queryParams.Search.ToLower())

                );
            }

            if (queryParams.DepartmentId.HasValue)
            {
                // tìm kiếm user sao trong bảng employeedepartment soa cho 
                //endate == null
                query = query.Include(x => x.DepartmentHistories)
                    .ThenInclude(x => x.Department)
                    .Where(x => x.DepartmentHistories.Any(x => x.EndDate == null &&
                    x.Department.DepartmentID == queryParams.DepartmentId));



            }
            if (queryParams.PositionId.HasValue)
            {
                // tìm kiếm user sao trong bảng employeedepartment soa cho 
                //endate == null
                query = query.Include(x => x.DepartmentHistories)
                    .ThenInclude(x => x.Position)
                    .Where(x => x.DepartmentHistories.Any(x => x.EndDate == null &&
                    x.Position.PositionID == queryParams.PositionId));



            }

            if (!string.IsNullOrWhiteSpace(queryParams.Gender))
            {

                query = query.Where(e => e.Gender == queryParams.Gender);


            }




            var totalCount = await query.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalCount / (double)queryParams.PageSize);
            var users = await query
                .OrderBy(e => e.UserID)
                .Skip((queryParams.CurrentPage - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();


            var userDTOlist = users.Select(x => new UserDTO
            {
                UserID = x.UserID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Address = x.Address,
                Gender = x.Gender,
                Phone = x.Phone,
                DateJoined = x.DateJoined,
                IsActive = x.IsActive,

                DepartmentName = _dbcontext.EmployeeDepartmentHistories
                .Include(x => x.Department)
               .Where(a => a.UserID == x.UserID && a.EndDate == null)
               .Select(z => z.Department.DepartmentName).FirstOrDefault(),
                PositionName = _dbcontext.EmployeeDepartmentHistories
                .Include(x => x.Position)
               .Where(a => a.UserID == x.UserID && a.EndDate == null)
               .Select(z => z.Position.PositionName).FirstOrDefault(),

                SalaryLevelName = _dbcontext.SalaryLevels
                .FirstOrDefault(a => a.SalaryLevelId == x.SalaryLevelId).LevelName,
                RoleName = _roleController.GetRoleUser(x) ?? null



            }).ToList();
            return Ok(new UserResponseQuery()
            {

                CurrentPage = queryParams.CurrentPage,
                PageSize = queryParams.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = userDTOlist


            });



        }



        //sủa nhân viên

        //thong tin chi tiet cua 1 nhan vien theo id



        // xóa nhân viên
        [HttpPut("disable/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var user = await _dbcontext.Users
                 .FirstOrDefaultAsync(x => x.UserID == id);
            if (user != null)
            {
                user.IsActive = false;
                _dbcontext.Users.Update(user);
                await _dbcontext.SaveChangesAsync();
                return Ok("vo hieu hoa user thanh cong");
            }

            return BadRequest("khong thanh cong");

        }

        // xóa nhân viên
        [HttpPut("isActive/{id}")]
        public async Task<IActionResult> IsActiveUser(int id)
        {
            var user = await _dbcontext.Users
                .FirstOrDefaultAsync(x => x.UserID == id);
            if (user != null)
            {
                user.IsActive = true;
                _dbcontext.Users.Update(user);
                await _dbcontext.SaveChangesAsync();
                return Ok("kich hoat user thanh cong");
            }

            return BadRequest("khong thanh cong");
        }


        // chuyển nhân viên sang ban ngành khác
        [HttpPut("{id}/transfer")]
        public async Task<IActionResult> TransferUser(int id, [FromBody] UserTranferDepartment dto)
        {
            var users = await _dbcontext.Users.FindAsync(id);
            if (users == null)
                return NotFound("usser không tồn tại");

            // Đóng bản ghi lịch sử công tác hiện tại (EndDate = hiện tại) nếu có
            var currentHistory = await _dbcontext.EmployeeDepartmentHistories
                .FirstOrDefaultAsync(h => h.UserID == id && h.EndDate == null);
            if (currentHistory != null)
            {
                // lưu í nếu mà phòng ban và chức vụ  giống hết hiện tại thì sẽ thông báo lỗi
                if (currentHistory.DepartmentID == dto.NewDepartmentID
                    && currentHistory.PositionID == dto.NewPositionID)
                {
                    return BadRequest("không được trùng nhau");
                }
                else currentHistory.EndDate = DateTime.Now;
            }





            // Tạo bản ghi mới cho chuyển phòng/chức vụ
            var newHistory = new EmployeeDepartmentHistory
            {

                DepartmentID = dto.NewDepartmentID,
                PositionID = dto.NewPositionID,
                UserID = id,
                StartDate = DateTime.Now,
                EndDate = null
            };

            _dbcontext.EmployeeDepartmentHistories.Add(newHistory);
            await _dbcontext.SaveChangesAsync();
            return Ok("chuyen phong ban thanh cong");
        }

        [NonAction]
        public void sáda(User x)
        {



        }



    }
}
