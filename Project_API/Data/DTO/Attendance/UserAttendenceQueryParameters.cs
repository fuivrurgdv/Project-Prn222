namespace Project_API.Data.DTO.Attendance
{
    public class UserAttendenceQueryParameters
    {


        public DateTime? FromDateAttendence { get; set; }     // Giới tính
        public DateTime? ToDateAttendence { get; set; }     // Giới tính
        public int CurrentPage { get; set; } = 1;       // Trang hiện tại, mặc định = 1
        public int PageSize { get; set; } = 5;         // S
    }
}
