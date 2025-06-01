using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;
using WebsiteLaptop.Services;

namespace WebsiteLaptop.Areas.User.Controllers
{
    [Area("User")]
    public class LaptopController : Controller
    {
        private readonly WebsiteLaptopContext _context;
        private readonly LaptopSearchService _searchService;

        public LaptopController(WebsiteLaptopContext context, LaptopSearchService searchService)
        {
            _context = context;
            _searchService = searchService;
        }

        [HttpGet]
        [HttpGet]
        public IActionResult DanhSach(string? q, int? categoryId)
        { 
            var query = _context.Laptop
                .Include(l => l.HinhAnhLaptops)
                .Include(l => l.DanhMuc)
                .Include(l => l.ThongSoKyThuat)
                .Where(l => !l.DaXoa)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim().ToLower(); // ✅ hạ về chữ thường

                query = query.Where(l =>
                    l.TenLaptop.ToLower().Contains(q) ||
                    l.HangSanXuat.ToLower().Contains(q));
            }
            if (categoryId.HasValue)
                query = query.Where(l => l.MaDanhMuc == categoryId.Value);
            ViewBag.TuKhoa = q;
            ViewBag.CategoryId = categoryId;    
            return View(query.OrderByDescending(l => l.NgayTao).ToList());
        }


        [HttpGet]
        public async Task<IActionResult> ChiTiet(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop
                .AsNoTracking()
                .Include(l => l.DanhMuc)
                .Include(l => l.ThongSoKyThuat)
                .Include(l => l.HinhAnhLaptops)
                .FirstOrDefaultAsync(l => l.MaLaptop == id && !l.DaXoa);

            if (laptop == null) return NotFound();

            return View(laptop);
        }

        [HttpGet]
        public IActionResult QuanLy()
        {
            var laptops = _context.Laptop
                .Include(l => l.DanhMuc)
                .Where(l => !l.DaXoa)
                .OrderByDescending(l => l.NgayTao)
                .ToList();

            return View(laptops);
        }

        [HttpGet]
        public IActionResult SanPhamDaAn()
        {
            var laptops = _context.Laptop
                .Include(l => l.DanhMuc)
                .Where(l => l.DaXoa)
                .ToList();

            return View(laptops);
        }

        [HttpPost]
        public IActionResult AnSanPham(int id)
        {
            var laptop = _context.Laptop.Find(id);
            if (laptop == null) return NotFound();

            laptop.DaXoa = true;
            _context.SaveChanges();

            return RedirectToAction(nameof(QuanLy));
        }

        [HttpPost]
        public IActionResult KhoiPhucSanPham(int id)
        {
            var laptop = _context.Laptop.Find(id);
            if (laptop == null) return NotFound();

            laptop.DaXoa = false;
            _context.SaveChanges();

            return RedirectToAction(nameof(SanPhamDaAn));
        }

        [HttpGet]
        public IActionResult SuaThongSo(int id)
        {
            var ts = _context.ThongSoKyThuat.FirstOrDefault(t => t.MaLaptop == id);
            if (ts == null) return NotFound();
            return View(ts);
        }

        [HttpPost]
        public IActionResult SuaThongSo(ThongSoKyThuat ts)
        {
            if (!ModelState.IsValid)
                return View(ts);

            _context.ThongSoKyThuat.Update(ts);
            _context.SaveChanges();
            return RedirectToAction("ChiTiet", new { id = ts.MaLaptop });
        }
    }
}
