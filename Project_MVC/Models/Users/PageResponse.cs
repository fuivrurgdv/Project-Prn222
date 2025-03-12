using Project_MVC.Models.Department;
using Project_MVC.Models.Position;

namespace Project_MVC.Models.Users
{
    public class PageResponse
    {
        public UserQueryParameters UserQueryParameters { get; set; }
        public UserResponseQuery UserResponseQuery { get; set; }

        public List<DepartmentDTO> ListDepartment { get; set; }

        public List<PositionDTO> ListPosition { get; set; } 

    }
}
