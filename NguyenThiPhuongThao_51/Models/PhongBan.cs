using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenThiPhuongThao_51.Models
{
    [Table("PhongBans")]
    public class PhongBan
    {
    [Key]
        public int MaPB{ get; set; }
        public string TenPB { get; set; }
    }
}