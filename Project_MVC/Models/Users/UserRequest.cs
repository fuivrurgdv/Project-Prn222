using System.ComponentModel.DataAnnotations;

namespace Project_MVC.Models.Users
{
    public class UserRequest
    {


        [MaxLength(50)]
        public string FirstName { get; set; } // Họ 


        [MaxLength(50)]
        public string LastName { get; set; } // Tên 

        public DateTime DateOfBirth { get; set; } // Ngày sinh

        [MaxLength(200)]
        public string Address { get; set; } // Địa chỉ

        [MaxLength(200)]
        public string Gender { get; set; } // giới tính
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } // Email
        [Required]
        public string Password { get; set; } // mật khẩu

        [MaxLength(20)]
        public string Phone { get; set; } // Số điện thoại

        //tên mức lương
        [Required]
        public int SalaryLevelId { get; set; }
        [Required]
        //têm phòng ban
        public int DepartmentId { get; set; }
        [Required]
        //tên chức vụ
        public int PositionId { get; set; }



    }
}
