using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data.dbContext;
using Project_API.Data.Model;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public RoleController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }

        // danh sách các role của 1 user

        [NonAction]
        public string? GetRoleUser(User a)
        {
            var role = _dbcontext.UserRoles
                .Where(x => x.UserID == a.UserID)
                .Include(x => x.Role)
                .Select(x => x.Role.RoleName)
                .ToList();
            if (role.Count() != 0)
            {
                string s = "";
                foreach (var x in role)
                {
                    s += x + ",";
                }
                s = s.Substring(0, s.Length - 1);
                return s;

            }
            return null;

        }

        //// thêm role 
        //[HttpPost]
        //public async Task<IActionResult> Addrole([FromBody] RoleRequest request)
        //{

        //}
        //// sửa role
        //[HttpPost("{id}")]
        //public async Task<IActionResult> Addrole([FromRoute] int id, [FromBody] RoleRequest request)
        //{

        //}

        //// laays danh sacsh cacs role
        //[HttpGet]
        //public async Task<IActionResult> GetRole()
        //{
        //    var role = await _dbcontext.Roles.ToListAsync();
           
        //} 


    }
}
