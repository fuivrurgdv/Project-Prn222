using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Project_API.Data.Model
{
    public class ShiftSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftSettingID { get; set; }      // Mã cấu hình ca làm việc

       
        [Required(ErrorMessage = "Giờ vào là bắt buộc")]
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }      // Giờ bắt đầu làm việc theo ca (vd: 08:45)

        [Required(ErrorMessage = "Giờ ra là bắt buộc")]
        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }     // Giờ kết thúc làm việc theo ca (vd: 17:00)

        public bool IsActive { get; set; }
        public DateTime CreateDay { get; set; } 

    }
}
