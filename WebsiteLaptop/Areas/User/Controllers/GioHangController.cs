using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;
using System.Security.Claims;

namespace WebsiteLaptop.Controllers
{
    [Area("User")]
    public class GioHangController : Controller
    {
        private readonly WebsiteLaptopContext _context;

        public GioHangController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        private int LayMaNguoiDung()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        // Hiển thị giỏ hàng
        public async Task<IActionResult> GioHang()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("DangNhap", "TaiKhoan");

            var maNguoiDung = LayMaNguoiDung();

            var gioHang = await _context.ChiTietGioHang
                .Include(g => g.Laptop)
                .ThenInclude(l => l.HinhAnhLaptops)
                .Where(g => g.MaNguoiDung == maNguoiDung)
                .ToListAsync();

            return View("GioHang", gioHang);
        }

        // Thêm sản phẩm vào giỏ
        public async Task<IActionResult> ThemVaoGio(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("DangNhap", "TaiKhoan");

            var maNguoiDung = LayMaNguoiDung();

            var gioHang = await _context.ChiTietGioHang
                .FirstOrDefaultAsync(g => g.MaNguoiDung == maNguoiDung && g.MaLaptop == id);

            if (gioHang != null)
            {
                gioHang.SoLuong += 1;
                gioHang.NgayThem = DateTime.Now;
            }
            else
            {
                gioHang = new ChiTietGioHang
                {
                    MaNguoiDung = maNguoiDung,
                    MaLaptop = id,
                    SoLuong = 1,
                    NgayThem = DateTime.Now
                };
                _context.ChiTietGioHang.Add(gioHang);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("GioHang");
        }

        // Xoá 1 mục
        public async Task<IActionResult> Xoa(int id)
        {
            var item = await _context.ChiTietGioHang.FindAsync(id);
            if (item != null)
            {
                _context.ChiTietGioHang.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("GioHang");
        }

        // Cập nhật số lượng
        [HttpPost]
        public async Task<IActionResult> CapNhatSoLuong(int id, int soLuong)
        {
            var item = await _context.ChiTietGioHang.FindAsync(id);
            if (item != null && soLuong > 0)
            {
                item.SoLuong = soLuong;
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("GioHang");
        }
    }
}
