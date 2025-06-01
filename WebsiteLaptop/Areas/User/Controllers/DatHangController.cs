using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

namespace WebsiteLaptop.Controllers
{
    [Area("User")]
    public class DatHangController : Controller
    {
        private readonly WebsiteLaptopContext _context;

        public DatHangController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        private int LayMaNguoiDung()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpPost]
        public async Task<IActionResult> TaoDon(string sanPhamIds)
        {
            if (string.IsNullOrWhiteSpace(sanPhamIds)) return RedirectToAction("Index", "GioHang");

            var ids = sanPhamIds.Split(',').Select(int.Parse).ToList();
            var userId = LayMaNguoiDung();

            var sanPhams = await _context.ChiTietGioHang
                .Include(x => x.Laptop)
                .ThenInclude(l => l.HinhAnhLaptops)
                .Where(x => ids.Contains(x.MaChiTietGioHang) && x.MaNguoiDung == userId)
                .ToListAsync();

            return View("DatHang", sanPhams);
        }
        [HttpPost]
        public async Task<IActionResult> XacNhan()
        {
            var userId = LayMaNguoiDung();

            var gioHang = await _context.ChiTietGioHang
                .Include(c => c.Laptop)
                .Where(c => c.MaNguoiDung == userId)
                .ToListAsync();

            if (!gioHang.Any())
                return RedirectToAction("Index", "GioHang");

            // Tính tổng tiền
            var tongTien = gioHang.Sum(c => c.SoLuong * c.Laptop.Gia);

            // Tạo đơn hàng mới
            var donHang = new DonHang
            {
                MaNguoiDung = userId,
                NgayDatHang = DateTime.UtcNow,
                TrangThai = "Chờ xử lý",
                TongTien = tongTien,
                DiaChiGiaoHang = "", // Có thể dùng form để người dùng nhập
                DienThoaiGiaoHang = "" // Tương tự
            };
            _context.DonHang.Add(donHang);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng
            foreach (var item in gioHang)
            {
                var chiTiet = new DonHangChiTiet
                {
                    MaDonHang = donHang.MaDonHang,
                    MaLaptop = item.MaLaptop,
                    SoLuong = item.SoLuong,
                    DonGia = item.Laptop.Gia
                };
                _context.DonHangChiTiet.Add(chiTiet);
            }

            // Xoá giỏ hàng
            _context.ChiTietGioHang.RemoveRange(gioHang);
            await _context.SaveChangesAsync();

            // Chuyển hướng về danh sách đơn hàng Admin để kiểm tra (hoặc tạo trang cảm ơn)
            return RedirectToAction("CamOn"); // ✅ Đúng
        }
        [HttpGet]
        public IActionResult CamOn()
        {
            return View();
        }


    }
}
