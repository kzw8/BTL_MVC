using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;      // namespace chứa DbContext
using WebsiteLaptop.Models;    // namespace chứa Laptop

namespace WebsiteLaptop.Controllers
{
    public class LaptopController : Controller
    {
        private readonly WebsiteLaptopContext _context;

        public LaptopController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        // GET: /Laptop
        public async Task<IActionResult> Index()
        {
            var list = await _context.Laptop
                                     .AsNoTracking()
                                     .ToListAsync();
            return View(list);
        }

        // GET: /Laptop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.MaLaptop == id);
            if (laptop == null) return NotFound();

            return View(laptop);
        }

        // GET: /Laptop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Laptop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ten,Gia,MoTa,DanhMucId,AnhUrl,…")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);
        }

        // GET: /Laptop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop.FindAsync(id);
            if (laptop == null) return NotFound();

            return View(laptop);
        }

        // POST: /Laptop/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,Gia,MoTa,DanhMucId,AnhUrl,…")] Laptop laptop)
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
                    if (!LaptopExists(laptop.MaLaptop))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);
        }

        // GET: /Laptop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptop
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.MaLaptop == id);
            if (laptop == null) return NotFound();

            return View(laptop);
        }

        // POST: /Laptop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptop = await _context.Laptop.FindAsync(id);
            _context.Laptop.Remove(laptop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptop.Any(e => e.MaLaptop == id);
        }
    }
}
