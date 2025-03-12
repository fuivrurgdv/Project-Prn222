

using Project_MVC.Models.History;

namespace Project_MVC.Models.Users
{
    public class UserDTOView
    {
        public UserDTO UserDTO { get; set; }

        public  List<HistoryWork>  HistoryWork { get; set; }
    }
}
