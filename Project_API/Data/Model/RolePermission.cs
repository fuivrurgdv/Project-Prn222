using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class RolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionID { get; set; } // Mã phân quyền (PK)


        public int RoleID { get; set; } // Mã vai trò (FK)
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }


        public int PermissionID { get; set; } // Mã quyền (FK)
        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }
    }
}
