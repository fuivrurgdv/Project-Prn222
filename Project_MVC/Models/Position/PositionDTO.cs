﻿using System.ComponentModel.DataAnnotations;

namespace Project_MVC.Models.Position
{
    public class PositionDTO
    {

        public int PositionID { get; set; }
        [Required]
        [MaxLength(100)]
        public string PositionName { get; set; } // Tên chức vụ
        [Required]
        [MaxLength(100)]
        public string Description { get; set; } // Mô tả chức vụ
        [Required]

        public bool IsActive { get; set; }
        [Required]

        public double PositionAllowance { get; set; } // Phụ cấp chức vụ (tiền cố định)
    }
}
