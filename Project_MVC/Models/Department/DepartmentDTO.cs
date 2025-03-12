using System.ComponentModel.DataAnnotations;

namespace Project_MVC.Models.Department
{
    public class DepartmentDTO
    {
        public int DepartmentID { get; set; } // Mã phòng ban (PK)

        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; } // Tên phòng ban

        [Required]
        [MaxLength(100)]
        public string AdressDepartment { get; set; } // địa chỉ phòng ban

        public string Description { get; set; } // Mô tả phòng ban

        public bool IsActive { get; set; }
    }
}
