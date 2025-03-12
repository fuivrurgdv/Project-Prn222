namespace Project_MVC.Models.Users
{
    public class UserResponseQuery
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public List<UserDTO> Data { get; set; }
    }
}
