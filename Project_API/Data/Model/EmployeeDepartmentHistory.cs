using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class EmployeeDepartmentHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeDepartmentHistoryID { get; set; } // Mã lịch sử (PK)

        [ForeignKey("UserID")]
        public int UserID { get; set; } // Mã nhân viên (FK)
        public virtual User User { get; set; }

        [ForeignKey("DepartmentID")]
        public int DepartmentID { get; set; } // Mã phòng ban (FK)
        public virtual Department Department { get; set; }

        [ForeignKey("PositionID")]
        public int PositionID { get; set; } // Mã chức vụ (FK)
        public virtual Position Position { get; set; }

       

        public DateTime StartDate { get; set; } // Ngày bắt đầu làm việc tại phòng ban này
        public DateTime? EndDate { get; set; } // Ngày kết thúc (nếu null, hiện tại)
    }
}
