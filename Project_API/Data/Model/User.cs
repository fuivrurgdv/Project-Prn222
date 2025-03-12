using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; } // Mã nhân viên (PK)

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } // Họ 

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } // Tên 

        public DateTime DateOfBirth { get; set; } // Ngày sinh

        [MaxLength(200)]
        public string Address { get; set; } // Địa chỉ
        [MaxLength(200)]
        public string Gender { get; set; } // giới tính

        [MaxLength(100)]
        public string Username { get; set; } // Email

        public string Password { get; set; } // mật khẩu

        [MaxLength(20)]
        public string Phone { get; set; } // Số điện thoại

        public DateTime DateJoined { get; set; } // Ngày vào làm (dùng để tính thâm niên)

        public bool IsActive { get; set; }

        public int SalaryLevelId { get; set; }  // Mã nhân viên (PK)
        [ForeignKey("SalaryLevelId")]
        public virtual SalaryLevel SalaryLevel { get; set; }

        // Navigation Properties
        public virtual ICollection<EmployeeDepartmentHistory> DepartmentHistories { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }



    }
}
