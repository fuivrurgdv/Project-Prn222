namespace Project_MVC.Models.History
{
    public class HistoryWork
    {
        public string DepartmentName { get; set; } // Mã phòng ban (FK)



        public string PositionName { get; set; } // Mã chức vụ (FK)

        public DateTime StartDate { get; set; } // Ngày bắt đầu làm việc tại phòng ban này
        public DateTime? EndDate { get; set; } // Ngày kết thúc (nếu null, hiện tại)
    }
}
