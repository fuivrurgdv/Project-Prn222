using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_API.Data.Model
{
    public class LeaveRequests
    {
        [Key][Required]
        public int RequestID { get; set; }

        public DateTime? StartDate { get; set; }               // Giờ vào thực tế
        public DateTime? EndDate { get; set; }

        public string? LeaveType { get; set; }

        public string? Reason { get; set; }
        public Boolean? isPaid { get; set; }

        public int? duration {  get; set; }
        public int? status { get; set; }

        public int? ApproveBy {  get; set; }

        public int UserID { get; set; }        // Mã nhân viên (FK)
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
