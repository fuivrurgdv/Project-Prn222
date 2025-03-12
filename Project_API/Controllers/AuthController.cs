using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_API.Data.dbContext;
using Project_API.Data.DTO.Login;
using Project_API.Data.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly MyDBContext _dbcontext;
        public AuthController(IConfiguration configuration, MyDBContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }


        // login
        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // nếu ko valiation
            if (!ModelState.IsValid)
            {
                return BadRequest("thông tin đan nhập ko chính xác");
            }

            // Tìm user 
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Username == dto.Username && x.IsActive == true && x.Password == dto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tài khoản không tồn tại." });
            }
            string token = GenerateToken(user);
            return Ok(new
            {
                Token = token
            });



        }

        //generate ra  token 
        private string GenerateToken(User user)
        {
            //  lấy cấu hình JWT từ appseting  json
            var jwtSettings = _configuration.GetSection("Jwt");
            // lấy thời gian hết hạn token 
            var expireMinutes = int.Parse(jwtSettings["ExpireMinutes"]);

            //tạo creds
            var secretKey = jwtSettings["Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // tạo list các claim
            var claims = new List<Claim>
              {


                         new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),




              };

            //tim kiếm các role của user và thêm vào claim 
            int id = user.UserID;
            var roles = _dbcontext.UserRoles.Where(x => x.UserID == id)
                .Include(x => x.Role).Select(x => x.Role.RoleName).ToList();
            foreach (var x in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, x));
            }

            // tìm kiếm các permission của user và thêm vào claim
            var permisison = _dbcontext.UserRoles.Where(x => x.UserID == id)
                .SelectMany(x => x.Role.RolePermissions.Select(x => x.Permission.PermissionName))
                .Distinct()
                .ToList();
            foreach (var x in permisison)
            {
                claims.Add(new Claim("Permission", x));
            }




            // thiết lập token với các tham số
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        

    }
}
