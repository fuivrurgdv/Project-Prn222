using System.ComponentModel.DataAnnotations;

namespace Project_MVC.Models.Setting
{
    public class SettingRequest
    {
        [Required]
        public DateTime ClockInTime { get; set; }      // Giờ bắt đầu làm việc theo ca (vd: 08:45)

        [Required]
        public DateTime ClockOutTime { get; set; }     // Giờ kết thúc làm việc theo ca (vd: 17:00)

       
    }
}
