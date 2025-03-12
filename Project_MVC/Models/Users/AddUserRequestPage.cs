using Project_MVC.Models.Department;
using Project_MVC.Models.Position;
using Project_MVC.Models.SalaryLevel;

namespace Project_MVC.Models.Users
{
    public class AddUserRequestPage
    {
        public UserRequest UserRequest { get; set; }
        public List<DepartmentDTO> ListDepartment { get; set; } = new List<DepartmentDTO>();

        public List<SalaryLevelDTO> ListSalaryLevelName { get; set; } = new List<SalaryLevelDTO>();

        public List<PositionDTO>? ListPosition { get; set; } = new List<PositionDTO>();
    }
}
