using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class UserRole
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleID { get; set; } // Mã phân quyền của người dùng (PK)

        [ForeignKey("UserID")]
        public int UserID { get; set; } // Mã nhân viên (FK)
        public virtual User User { get; set; }

       
        public int RoleID { get; set; } // Mã vai trò (FK)
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }
}
