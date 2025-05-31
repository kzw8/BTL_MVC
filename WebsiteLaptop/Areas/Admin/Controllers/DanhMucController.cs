using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DanhMucController : Controller
{
    private readonly WebsiteLaptopContext _context;

    public DanhMucController(WebsiteLaptopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("DanhSach");
    }


    [HttpGet]
    public IActionResult DanhSach()
    {
        var danhMuc = _context.DanhMuc.Where(dm => !dm.DaXoa).ToList();
        return View("DanhSach", danhMuc);
    }

    [HttpGet]
    public IActionResult TaoMoi()
    {
        return View("TaoMoi");
    }

    [HttpPost]
    public IActionResult TaoMoi(DanhMuc model)
    {
        if (!ModelState.IsValid) return View("TaoMoi", model);

        _context.DanhMuc.Add(model);
        _context.SaveChanges();
        return RedirectToAction("DanhSach");
    }

    [HttpGet]
    public IActionResult CapNhat(int id)
    {
        var dm = _context.DanhMuc.Find(id);
        if (dm == null || dm.DaXoa) return NotFound();

        return View("CapNhat", dm);
    }

    [HttpPost]
    public IActionResult CapNhat(DanhMuc model)
    {
        if (!ModelState.IsValid) return View("CapNhat", model);

        var dm = _context.DanhMuc.Find(model.MaDanhMuc);
        if (dm == null) return NotFound();

        dm.TenDanhMuc = model.TenDanhMuc;
        dm.MoTa = model.MoTa;
        _context.SaveChanges();

        return RedirectToAction("DanhSach");
    }

    [HttpGet]
    public IActionResult AnDanhMuc(int id)
    {
        var dm = _context.DanhMuc.Find(id);
        if (dm == null) return NotFound();

        dm.DaXoa = true;
        dm.NgayXoa = DateTime.Now;
        _context.SaveChanges();
        return RedirectToAction("DanhSach");
    }

    [HttpGet]
    public IActionResult DanhMucDaAn()
    {
        var list = _context.DanhMuc.Where(dm => dm.DaXoa).ToList();
        return View("DanhMucDaAn", list);
    }

    [HttpGet]
    public IActionResult KhoiPhucDanhMuc(int id)
    {
        var dm = _context.DanhMuc.Find(id);
        if (dm == null || !dm.DaXoa) return NotFound();

        dm.DaXoa = false;
        dm.NgayXoa = null;
        _context.SaveChanges();
        return RedirectToAction("DanhMucDaAn");
    }

    [HttpPost]
    public IActionResult AnNhieu(List<int> ids)
    {
        var danhMuc = _context.DanhMuc.Where(dm => ids.Contains(dm.MaDanhMuc)).ToList();
        foreach (var dm in danhMuc)
        {
            dm.DaXoa = true;
            dm.NgayXoa = DateTime.Now;
        }
        _context.SaveChanges();
        TempData["Success"] = $"Đã ẩn {danhMuc.Count} danh mục.";
        return RedirectToAction("DanhSach");
    }

    [HttpPost]
    public IActionResult KhoiPhucNhieu(List<int> ids)
    {
        var danhMuc = _context.DanhMuc.Where(dm => ids.Contains(dm.MaDanhMuc)).ToList();
        foreach (var dm in danhMuc)
        {
            dm.DaXoa = false;
            dm.NgayXoa = null;
        }
        _context.SaveChanges();
        TempData["Success"] = $"Đã khôi phục {danhMuc.Count} danh mục.";
        return RedirectToAction("DanhMucDaAn");
    }
}
