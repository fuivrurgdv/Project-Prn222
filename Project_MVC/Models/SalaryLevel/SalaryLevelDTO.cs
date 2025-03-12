namespace Project_MVC.Models.SalaryLevel
{
    public class SalaryLevelDTO
    {
        public int SalaryLevelId { get; set; }  // Mã mức lương (PK)


        public string LevelName { get; set; }   // Tên mức lương (ví dụ: "Mức 1", "Mức 2", ...)


        public double BasicSalary { get; set; } // Lương cơ bản cho mức này (ví dụ: 5000000)

        public string Description { get; set; } // Mô tả (nếu cần)
    }
}
