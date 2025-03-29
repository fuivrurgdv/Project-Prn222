using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.DTO.SalaryLevel
{
    public class SalaryRequest
    {
        


            [Required]
            public string LevelName { get; set; }   // Tên mức lương (ví dụ: "Mức 1", "Mức 2", ...)

            [Required]
            public double BasicSalary { get; set; } // Lương cơ bản cho mức này (ví dụ: 5000000)
            [Required]
            public string Description { get; set; } // Mô tả (nếu cần)
        
    }
}
