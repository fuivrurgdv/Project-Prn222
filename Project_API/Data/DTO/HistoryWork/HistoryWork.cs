using Project_API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.DTO.HistoryWork
{
    public class HistoryWork
    {



        public string DepartmentName { get; set; } // Mã phòng ban (FK)



        public string PositionName { get; set; } // Mã chức vụ (FK)

        public DateTime StartDate { get; set; } // Ngày bắt đầu làm việc tại phòng ban này
        public DateTime? EndDate { get; set; } // Ngày kết thúc (nếu null, hiện tại)

    }
}
