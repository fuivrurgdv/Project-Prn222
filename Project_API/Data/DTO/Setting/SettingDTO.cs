using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.DTO.Setting
{
    public class SettingDTO
    {

        public int ShiftSettingID { get; set; }      // Mã cấu hình ca làm việc

        [Required]
        public DateTime ClockInTime { get; set; }      // Giờ bắt đầu làm việc theo ca (vd: 08:45)

        [Required]
        public DateTime ClockOutTime { get; set; }     // Giờ kết thúc làm việc theo ca (vd: 17:00)

        public bool IsActive { get; set; }
        public DateTime CreateDay { get; set; }
    }
}
