using Microsoft.EntityFrameworkCore;
using Project_API.Data.Model;

namespace Project_API.Data.dbContext
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions option) : base(option) { }

        public DbSet<ShiftSetting> ShiftSettings { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<SalaryLevel> SalaryLevels { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<LeaveRequests> LeaveRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeDepartmentHistory>()
                .HasOne(ed => ed.Position)
                .WithMany(p => p.EmployeeDepartmentHistories)
                .HasForeignKey(ed => ed.PositionID)
                .OnDelete(DeleteBehavior.Restrict);

            // Tương tự, nếu có các quan hệ khác gây cascade path, cấu hình chúng cũng như vậy.

            //modelBuilder.Entity<User>()
            //    .HasOne(e => e.SalaryLevel)
            //    .WithMany(s => s.Users)
            //    .HasForeignKey(e => e.SalaryLevelId)
            //    .OnDelete(DeleteBehavior.Restrict); // hoặc DeleteBehavior.NoAction


        }

    }
}
