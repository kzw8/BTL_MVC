using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

namespace WebsiteLaptop.Areas.User.Controllers
{
    [Area("User")]
    public class LaptopController : Controller
    {
        private readonly WebsiteLaptopContext _context;

        public LaptopController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> DanhSach()
        {
            var list = await _context.Laptop.AsNoTracking().ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> ChiTiet(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaLaptop == id);

            if (laptop == null) return NotFound();

            return View(laptop);
        }

        [HttpGet]
        public IActionResult TaoMoi()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoMoi([Bind("Id,Ten,Gia,MoTa,DanhMucId,AnhUrl")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DanhSach));
            }
            return View(laptop);
        }

        [HttpGet]
        public async Task<IActionResult> ChinhSua(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop.FindAsync(id);
            if (laptop == null) return NotFound();

            return View(laptop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSua(int id, [Bind("Id,Ten,Gia,MoTa,DanhMucId,AnhUrl")] Laptop laptop)
        {
            if (id != laptop.MaLaptop) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Laptop.Any(e => e.MaLaptop == laptop.MaLaptop))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(DanhSach));
            }
            return View(laptop);
        }

        [HttpGet]
        public async Task<IActionResult> Xoa(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaLaptop == id);

            if (laptop == null) return NotFound();

            return View(laptop);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaXacNhan(int id)
        {
            var laptop = await _context.Laptop.FindAsync(id);
            _context.Laptop.Remove(laptop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DanhSach));
        }

        [HttpGet]
        public IActionResult QuanLy()
        {
            var laptops = _context.Laptop
                .Include(l => l.DanhMuc)
                .OrderByDescending(l => l.NgayTao)
                .ToList();

            return View(laptops);
        }

        [HttpGet]
        public IActionResult SuaThongSo(int id)
        {
            var ts = _context.ThongSoKyThuat.FirstOrDefault(t => t.MaLaptop == id);
            if (ts == null) return NotFound();
            return View(ts);
        }

        [HttpPost, ValidateAntiForgeryToken]
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