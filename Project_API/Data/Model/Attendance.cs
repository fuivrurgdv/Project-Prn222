using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceID { get; set; }               // Mã chấm công (PK)

        public DateTime WorkDate { get; set; }              // Ngày làm việc
        public DateTime? ClockIn { get; set; }               // Giờ vào thực tế
        public DateTime? ClockOut { get; set; }             // Giờ ra thực tế (nullable khi chưa Clock Out)
        public DateTime ScheduledClockIn { get; set; }      // Giờ vào dự kiến theo ca làm việc
        public DateTime? ScheduledClockOut { get; set; }    // Giờ ra dự kiến theo ca làm việc (nullable)

        public double? ToTalHours { get; set; }





        // foreign key
        public int UserID { get; set; }        // Mã nhân viên (FK)
        [ForeignKey("UserID")]

        public virtual User User { get; set; }
    }
}
