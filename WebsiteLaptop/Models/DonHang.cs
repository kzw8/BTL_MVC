using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteLaptop.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        [ForeignKey(nameof(NguoiDung))]
        public int MaNguoiDung { get; set; }

        public DateTime NgayDatHang { get; set; }

        [Required, StringLength(20)]
        public string TrangThai { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        [StringLength(200)]
        public string? DiaChiGiaoHang { get; set; }

        [StringLength(20)]
        public string? DienThoaiGiaoHang { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
            = new List<DonHangChiTiet>();
    }
}
