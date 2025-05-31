using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteLaptop.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required, StringLength(100)]
        public string TenDanhMuc { get; set; } = string.Empty;

        [StringLength(500)]
        public string? MoTa { get; set; }
        public bool DaXoa { get; set; } = false;

        public DateTime? NgayXoa { get; set; }


        // Navigation: 1 DanhMuc có nhiều Laptop
        public virtual ICollection<Laptop> Laptops { get; set; }
            = new List<Laptop>();
        

    }
}
