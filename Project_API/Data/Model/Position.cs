using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionID { get; set; } // Mã chức vụ (PK)

        [Required]
        [MaxLength(100)]
        public string PositionName { get; set; } // Tên chức vụ

        public string Description { get; set; } // Mô tả chức vụ


        public int DepartmentID { get; set; } // Mã phòng ban (FK)
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        public bool IsActive { get; set; }
        public double PositionAllowance { get; set; } // Phụ cấp chức vụ (tiền cố định)

        // Navigation Property
        public virtual ICollection<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
    }
}
