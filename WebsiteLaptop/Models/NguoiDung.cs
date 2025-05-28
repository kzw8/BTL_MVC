using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteLaptop.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required, StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required, StringLength(256)]
        public string MatKhau { get; set; }

        [StringLength(100)]
        public string? HoVaTen { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string? DienThoai { get; set; }

        [StringLength(200)]
        public string? DiaChi { get; set; }

        [Required, StringLength(20)]
        public string VaiTro { get; set; }

        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
            = new List<ChiTietGioHang>();
        public virtual ICollection<DonHang> DonHangs { get; set; }
            = new List<DonHang>();
    }
}
