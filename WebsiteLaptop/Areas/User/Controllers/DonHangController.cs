using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebsiteLaptop.Data;

namespace WebsiteLaptop.Areas.User.Controllers
{
    [Area("User")]
    public class DonHangController : Controller
    {
        private readonly WebsiteLaptopContext _context;

        public DonHangController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        private int LayMaNguoiDung()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpGet]
        public IActionResult DanhSach()
        {
            var userId = LayMaNguoiDung();

            var donHangs = _context.DonHang
                .Include(d => d.DonHangChiTiets)
                    .ThenInclude(ct => ct.Laptop)
                .Where(d => d.MaNguoiDung == userId)
                .OrderByDescending(d => d.NgayDatHang)
                .ToList();

            return View(donHangs);
        }
        [HttpGet]
        public IActionResult ChiTiet(int id)
        {
            var userId = LayMaNguoiDung();

            var donHang = _context.DonHang
                .Include(d => d.DonHangChiTiets)
                    .ThenInclude(ct => ct.Laptop)
                        .ThenInclude(l => l.HinhAnhLaptops) // nếu cần ảnh
                .Include(d => d.NguoiDung)
                .FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiDung == userId);

            if (donHang == null)
                return NotFound();

            return View("ChiTiet", donHang); // View nằm tại /Areas/User/Views/DonHang/ChiTiet.cshtml
        }
        [HttpGet]
        public IActionResult InHoaDon(int id)
        {
            var userId = LayMaNguoiDung();

            var donHang = _context.DonHang
                .Include(d => d.DonHangChiTiets)
                    .ThenInclude(ct => ct.Laptop)
                .Include(d => d.NguoiDung)
                .FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiDung == userId);

            if (donHang == null) return NotFound();

            return View("InHoaDon", donHang); // View dành cho User
        }

    }
}
