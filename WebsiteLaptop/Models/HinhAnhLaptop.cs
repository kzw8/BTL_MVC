using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteLaptop.Models
{
    public class HinhAnhLaptop
    {
        [Key]
        public int MaAnh { get; set; }

        [Required]
        public string DuongDan { get; set; } = string.Empty;

        // Foreign Key
        public int MaLaptop { get; set; }

        [ForeignKey("MaLaptop")]
        public Laptop Laptop { get; set; }
    }
}