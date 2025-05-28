using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteLaptop.Models
{
    public class DonHangChiTiet
    {
        [Key]
        public int MaChiTietDonHang { get; set; }

        [ForeignKey(nameof(DonHang))]
        public int MaDonHang { get; set; }

        [ForeignKey(nameof(Laptop))]
        public int MaLaptop { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        public virtual DonHang DonHang { get; set; }
        public virtual Laptop Laptop { get; set; }
    }
}
