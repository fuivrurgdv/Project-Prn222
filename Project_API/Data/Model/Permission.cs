using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.Model
{
    public class Permission
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionID { get; set; } // Mã quyền (PK)

        [Required]
        [MaxLength(50)]
        public string PermissionName { get; set; } // Tên quyền (ví dụ: AddEmployee, EditAttendance, ProcessPayroll,...)

        public string Description { get; set; } // Mô tả quyền

        // Navigation
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
