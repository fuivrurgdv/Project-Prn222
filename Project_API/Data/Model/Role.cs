using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; } // Mã vai trò (PK)

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } // Tên vai trò (Admin, Employee, Manager,...)

        public string Description { get; set; } // Mô tả vai trò

      

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
