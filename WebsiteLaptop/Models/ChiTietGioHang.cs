using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteLaptop.Models
{
    public class ChiTietGioHang
    {
        [Key]
        public int MaChiTietGioHang { get; set; }

        [ForeignKey(nameof(NguoiDung))]
        public int MaNguoiDung { get; set; }

        [ForeignKey(nameof(Laptop))]
        public int MaLaptop { get; set; }

        public int SoLuong { get; set; }

        public DateTime NgayThem { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
        public virtual Laptop Laptop { get; set; }
    }
}
