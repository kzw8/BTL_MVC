using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

namespace WebsiteLaptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LaptopController : Controller
    {
        private readonly WebsiteLaptopContext _context;

        public LaptopController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        public IActionResult DanhSach()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Them()
        {
            ViewBag.DanhMucList = new SelectList(_context.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc");

            var hangList = _context.Laptop
                .Select(l => l.HangSanXuat)
                .Distinct()
                .Where(h => h != null && h != "")
                .ToList();

            if (!hangList.Any())
            {
                hangList = new List<string> { "ASUS", "Dell", "HP", "Acer", "Lenovo", "MSI", "MacBook", "Gigabyte" };
            }

            ViewBag.HangSanXuatList = new SelectList(hangList);

            return View("TaoMoi");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Them(Laptop laptop, List<IFormFile> HinhAnhFiles)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DanhMucList = new SelectList(_context.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc");
                return View(laptop);
            }

            laptop.NgayTao = DateTime.UtcNow;
            _context.Laptop.Add(laptop);
            _context.SaveChanges();

            if (HinhAnhFiles != null && HinhAnhFiles.Any())
            {
                foreach (var file in HinhAnhFiles)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/laptops", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var hinh = new HinhAnhLaptop
                    {
                        MaLaptop = laptop.MaLaptop,
                        DuongDan = "/images/laptops/" + fileName
                    };
                    _context.HinhAnhLaptop.Add(hinh);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("QuanLy");
        }

        [HttpGet]
        public IActionResult QuanLy()
        {
            var laptops = _context.Laptop
                .Include(l => l.DanhMuc)
                .Include(l => l.HinhAnhLaptops)
                .Where(l => !l.DaXoa)
                .ToList();
            return View(laptops);
        }

        //thong so ky thuat
        [HttpGet]
        public IActionResult ChiTiet(int id)
        {
            var laptop = _context.Laptop
                .Include(l => l.DanhMuc)
                .Include(l => l.HinhAnhLaptops)
                .Include(l => l.ThongSoKyThuat)
                .FirstOrDefault(l => l.MaLaptop == id);

            if (laptop == null)
                return NotFound();

            return View("ChiTiet", laptop);
        }

        //them thong so
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemThongSo(ThongSoKyThuat ts)
        {
            if (!ModelState.IsValid)
            {
                foreach (var err in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(err.ErrorMessage); // In ra lỗi
                }

                TempData["Error"] = "Model không hợp lệ!";
                return RedirectToAction("Detail", new { id = ts.MaLaptop });
            }

            _context.ThongSoKyThuat.Add(ts);
            _context.SaveChanges();

            TempData["Success"] = "Đã lưu thông số kỹ thuật";
            return RedirectToAction("ChiTiet", new { id = ts.MaLaptop });
        }

        //sua thong so
        [HttpGet]
        public IActionResult SuaThongSo(int id)
        {
            var laptop = _context.Laptop
                .Include(l => l.ThongSoKyThuat)
                .FirstOrDefault(l => l.MaLaptop == id);

            if (laptop == null) return NotFound();

            return View("ChiTiet", laptop); // Tạo thêm view nếu cần
        }

        // File: Areas/Admin/Controllers/LaptopController.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaThongSo(ThongSoKyThuat ts)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();

                TempData["Error"] = "Lỗi ModelState:\n" + string.Join("\n", errors);
                return RedirectToAction("ChiTiet", new { id = ts.MaLaptop });
            }


            if (ts.MaThongSo == 0)
            {
                _context.ThongSoKyThuat.Add(ts);
                TempData["Success"] = "Đã thêm thông số kỹ thuật.";
            }
            else
            {
                var thongSo = _context.ThongSoKyThuat.FirstOrDefault(x => x.MaThongSo == ts.MaThongSo);
                if (thongSo == null) return NotFound();

                thongSo.CPU = ts.CPU;
                thongSo.RAM = ts.RAM;
                thongSo.OCung = ts.OCung;
                thongSo.CardDoHoa = ts.CardDoHoa;
                thongSo.ManHinh = ts.ManHinh;
                thongSo.CongGiaoTiep = ts.CongGiaoTiep;
                thongSo.Audio = ts.Audio;
                thongSo.LAN = ts.LAN;
                thongSo.WIFI = ts.WIFI;
                thongSo.Bluetooth = ts.Bluetooth;
                thongSo.Webcam = ts.Webcam;
                thongSo.HeDieuHanh = ts.HeDieuHanh;
                thongSo.Pin = ts.Pin;
                thongSo.TrongLuong = ts.TrongLuong;

                TempData["Success"] = "Cập nhật thông số thành công.";
            }

            _context.SaveChanges();
            return RedirectToAction("ChiTiet", new { id = ts.MaLaptop });
        }


        //get edit
        [HttpGet]
        public IActionResult ChinhSua(int id)
        {
            var laptop = _context.Laptop
                .Include(l => l.HinhAnhLaptops)
                .FirstOrDefault(l => l.MaLaptop == id);

            if (laptop == null) return NotFound();

            ViewBag.DanhMucList = new SelectList(_context.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc", laptop.MaDanhMuc);
            return View(laptop);
        }


        //post edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ChinhSua(Laptop model, List<IFormFile> HinhAnhFiles, List<int> XoaAnhIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DanhMucList = new SelectList(_context.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
                return View(model);
            }

            var laptop = _context.Laptop
                .Include(l => l.HinhAnhLaptops)
                .FirstOrDefault(l => l.MaLaptop == model.MaLaptop);
            if (laptop == null) return NotFound();

            // Cập nhật thông tin
            laptop.TenLaptop = model.TenLaptop;
            laptop.HangSanXuat = model.HangSanXuat;
            laptop.Gia = model.Gia;
            laptop.MoTa = model.MoTa;
            laptop.MaDanhMuc = model.MaDanhMuc;

            // Xóa ảnh được chọn
            if (XoaAnhIds != null && XoaAnhIds.Any())
            {
                var anhCanXoa = laptop.HinhAnhLaptops.Where(h => XoaAnhIds.Contains(h.MaAnh)).ToList();
                foreach (var hinh in anhCanXoa)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hinh.DuongDan.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    _context.HinhAnhLaptop.Remove(hinh);
                }
            }

            // Thêm ảnh mới
            if (HinhAnhFiles != null && HinhAnhFiles.Any())
            {
                foreach (var file in HinhAnhFiles)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/laptops", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    _context.HinhAnhLaptop.Add(new HinhAnhLaptop
                    {
                        MaLaptop = laptop.MaLaptop,
                        DuongDan = "/images/laptops/" + fileName
                    });
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Cập nhật thành công.";
            return RedirectToAction("QuanLy");  
        }

        //delete
        [HttpGet]
        public IActionResult AnSanPham(int id)
        {
            var laptop = _context.Laptop.FirstOrDefault(l => l.MaLaptop == id);
            if (laptop == null) return NotFound();

            laptop.DaXoa = true;
            laptop.NgayXoa = DateTime.Now;
            _context.SaveChanges();

            TempData["Success"] = "Đã ẩn sản phẩm khỏi website.";
            return RedirectToAction("QuanLy");
        }


        //san pham da xoa
        [HttpGet]
        public IActionResult SanPhamDaAn(DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.Laptop
                .Include(l => l.DanhMuc)
                .Include(l => l.HinhAnhLaptops)
                .Where(l => l.DaXoa);

            if (tuNgay.HasValue)
                query = query.Where(l => l.NgayXoa >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(l => l.NgayXoa <= denNgay.Value);

            var laptops = query.ToList();
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            return View(laptops);
        }


        //khoi phuc
        [HttpGet]
        public IActionResult KhoiPhuc(int id)
        {
            var laptop = _context.Laptop.FirstOrDefault(l => l.MaLaptop == id && l.DaXoa);
            if (laptop == null) return NotFound();

            laptop.DaXoa = false;
            _context.SaveChanges();

            TempData["Success"] = "Đã khôi phục sản phẩm.";
            return RedirectToAction("SanPhamDaAn");
        }
        //khoi phuc nhieu
        [HttpPost]
        public IActionResult KhoiPhucNhieu(List<int> ids)
        {
            var laptops = _context.Laptop.Where(l => ids.Contains(l.MaLaptop) && l.DaXoa).ToList();
            foreach (var l in laptops)
            {
                l.DaXoa = false;
                l.NgayXoa = null;
            }
            _context.SaveChanges();
            TempData["Success"] = $"Đã khôi phục {laptops.Count} sản phẩm.";
            return RedirectToAction("SanPhamDaAn");
        }


    }
}
