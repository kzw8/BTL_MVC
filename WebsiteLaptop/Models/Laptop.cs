using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteLaptop.Models
{
    public class Laptop
    {
        [Key]
        public int MaLaptop { get; set; }

        [ForeignKey(nameof(DanhMuc))]
        public int MaDanhMuc { get; set; }

        [Required, StringLength(100)]
        public string TenLaptop { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string HangSanXuat { get; set; } = string.Empty ;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        public string? MoTa { get; set; }

        [StringLength(200)]
        public string? DuongDanAnh { get; set; }

        public DateTime NgayTao { get; set; }
        public bool DaXoa { get; set; } = false;
        public DateTime? NgayXoa { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; } = 1;

        public virtual DanhMuc? DanhMuc { get; set; }
        public virtual ThongSoKyThuat? ThongSoKyThuat { get; set; }


        public virtual ICollection<HinhAnhLaptop> HinhAnhLaptops { get; set; } = new List<HinhAnhLaptop>();
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
            = new List<ChiTietGioHang>();
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
            = new List<DonHangChiTiet>();
        public string TenDangNhap { get; set; } = string.Empty;

    }
}
