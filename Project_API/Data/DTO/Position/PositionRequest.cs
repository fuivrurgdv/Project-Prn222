using Project_API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.DTO.Position
{
    public class PositionRequest
    {


        [Required]
        [MaxLength(100)]
        public string PositionName { get; set; } // Tên chức vụ
        [Required]
        [MaxLength(100)]
        public string Description { get; set; } // Mô tả chức vụ
        [Required]
        public string DepartmentName { get; set; } 
        [Required]
       
        public double PositionAllowance { get; set; } // Phụ cấp chức vụ (tiền cố định)



    }
}
