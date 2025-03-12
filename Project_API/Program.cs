
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_API.Controllers;
using Project_API.Data.dbContext;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Project_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:5088")  // Chỉ cho phép origin của MVC
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // đăng kí các sẻvices

            // đăng kí dich vụ Dcontext 

            builder.Services.AddDbContext<MyDBContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddScoped<RoleController>();
            //cài đặt JWT  : 

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,                         // Kiểm tra Issuer
                    ValidateAudience = false,                       // Kiểm tra Audience
                    ValidateLifetime = true,                       // Kiểm tra thời gian hết hạn của token
                    ValidateIssuerSigningKey = true,               // Kiểm tra chữ ký của token

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero                      // Không cho phép sai lệch thời gian (mặc định 5 phút)
                };
            });



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
