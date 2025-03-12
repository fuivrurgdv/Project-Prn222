using System.ComponentModel.DataAnnotations;

namespace Project_MVC.Models.Users
{
    public class UserDTO
    {
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



        [MaxLength(20)]
        public string Phone { get; set; } // Số điện thoại

        public DateTime DateJoined { get; set; } // Ngày vào làm (dùng để tính thâm niên)

        public bool IsActive { get; set; }

        public string DepartmentName { get; set; }
        public string PositionName { get; set; }

        public string? SalaryLevelName { get; set; }

        public string? RoleName { get; set; }


    }
}
