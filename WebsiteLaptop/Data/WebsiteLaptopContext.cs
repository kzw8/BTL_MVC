using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Models;

namespace WebsiteLaptop.Data
{
    public class WebsiteLaptopContext : DbContext
    {
        public WebsiteLaptopContext(DbContextOptions<WebsiteLaptopContext> options)
            : base(options)
        {
        }

        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<Laptop> Laptop { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<DonHangChiTiet> DonHangChiTiet { get; set; }
        public DbSet<HinhAnhLaptop> HinhAnhLaptop { get; set; }
        public DbSet<ThongSoKyThuat> ThongSoKyThuat { get; set; }


    }
}
