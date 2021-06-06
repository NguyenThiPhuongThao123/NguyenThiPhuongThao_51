using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenThiPhuongThao_51.Models
{
    [Table("NhanViens")]
    public class NhanVien 
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public int MaPhongBan { get; set; }

    }
}