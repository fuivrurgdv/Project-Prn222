using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class SalaryLevel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryLevelId { get; set; }  // Mã mức lương (PK)

        [Required]
        [MaxLength(50)]
        public string LevelName { get; set; }   // Tên mức lương (ví dụ: "Mức 1", "Mức 2", ...)

        [Required]
        public double BasicSalary { get; set; } // Lương cơ bản cho mức này (ví dụ: 5000000)

        public string Description { get; set; } // Mô tả (nếu cần)

        public virtual ICollection<User> Users { get; set; }

    }
}
