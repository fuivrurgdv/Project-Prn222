namespace Project_MVC.Models.LeaveRequest
{
    public class SendLeaveRequest
    {

        public DateTime? StartDate { get; set; }               // Giờ vào thực tế
        public DateTime? EndDate { get; set; }

        public string? LeaveType { get; set; }

        public string? Reason { get; set; }
        public int? status { get; set; }

        public int UserID { get; set; }

        
    }
}
