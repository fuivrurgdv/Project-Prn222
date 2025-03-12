using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.DTO.Department
{
    public class DepartmentRequest
    {


        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; } // Tên phòng ban

        [Required]
        [MaxLength(100)]
        public string AdressDepartment { get; set; } // địa chỉ phòng ban

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } // Mô tả phòng ban

    }
}
