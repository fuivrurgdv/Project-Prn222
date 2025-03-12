namespace Project_API.Data.DTO.Users
{
    public class UserQueryParameters
    {
        public string? Search { get; set; }           // Từ khóa tìm kiếm (Họ Tên)
        public int? DepartmentId { get; set; }   // Tên phòng ban
        public int? PositionId { get; set; } // chúc vụ 
        public string? Gender { get; set; }     // Giới tính
        public int CurrentPage { get; set; } = 1;       // Trang hiện tại, mặc định = 1
        public int PageSize { get; set; } = 5;         // S
    }
}
