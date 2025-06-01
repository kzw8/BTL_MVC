using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteLaptop.Models
{
    public class ThongSoKyThuat
    {
        [Key]
        public int MaThongSo { get; set; }

        [Required]
        public int MaLaptop { get; set; }

        [ForeignKey("MaLaptop")]
        public Laptop? Laptop { get; set; }

        public string? CPU { get; set; } = string.Empty;
        public string? RAM { get; set; } = string.Empty;
        public string? OCung { get; set; } = string.Empty;
        public string? CardDoHoa { get; set; } = string.Empty;
        public string? ManHinh { get; set; } = string.Empty;
        public string? CongGiaoTiep { get; set; } = string.Empty;
        public string? Audio { get; set; } = string.Empty;
        public string? LAN { get; set; } = string.Empty;
        public string? WIFI { get; set; } = string.Empty;
        public string? Bluetooth { get; set; } = string.Empty;
        public string? Webcam { get; set; } = string.Empty;
        public string? HeDieuHanh { get; set; } = string.Empty;
        public string? Pin { get; set; } = string.Empty;
        public string? TrongLuong { get; set; } = string.Empty;
    }
}
