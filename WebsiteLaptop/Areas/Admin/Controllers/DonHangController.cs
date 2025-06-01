using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DonHangController : Controller
{
    private readonly WebsiteLaptopContext _context;

    public DonHangController(WebsiteLaptopContext context)
    {
        _context = context;
    }

    // ✅ Mặc định chuyển về danh sách
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("DanhSach");
    }

    // ✅ Danh sách tất cả đơn hàng
    [HttpGet]
    public IActionResult DanhSach(DateTime? tuNgay, DateTime? denNgay, string? trangThai)
    {
        var query = _context.DonHang
            .Include(d => d.NguoiDung)
            .AsQueryable();

        if (tuNgay.HasValue)
            query = query.Where(d => d.NgayDatHang >= tuNgay.Value);

        if (denNgay.HasValue)
            query = query.Where(d => d.NgayDatHang <= denNgay.Value);

        if (!string.IsNullOrEmpty(trangThai))
            query = query.Where(d => d.TrangThai == trangThai);

        var danhSach = query.OrderByDescending(d => d.NgayDatHang).ToList();

        ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
        ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");
        ViewBag.TrangThai = trangThai;

        return View("DanhSach", danhSach);
    }


    // ✅ Chi tiết 1 đơn hàng
    [HttpGet]
    public IActionResult ChiTiet(int id)
    {
        var donHang = _context.DonHang
            .Include(d => d.NguoiDung)
            .Include(d => d.DonHangChiTiets)
                .ThenInclude(ct => ct.Laptop)
            .FirstOrDefault(d => d.MaDonHang == id);

        if (donHang == null) return NotFound();

        return View("ChiTiet", donHang);
    }


    // ✅ Cập nhật trạng thái đơn hàng
    [HttpPost]
    public IActionResult CapNhatTrangThai(int id, string trangThai)
    {
        var donHang = _context.DonHang.Find(id);
        if (donHang == null) return NotFound();

        donHang.TrangThai = trangThai;
        _context.SaveChanges();

        TempData["Success"] = "✅ Đã cập nhật trạng thái đơn hàng.";
        return RedirectToAction("DanhSach");
    }
    public class LocDonHangDTO
    {
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string? TrangThai { get; set; }
    }

    //in hoa don
    [HttpGet]
    public IActionResult InHoaDon(int id)
    {
        var donHang = _context.DonHang
            .Include(d => d.NguoiDung)
            .Include(d => d.DonHangChiTiets)
                .ThenInclude(ct => ct.Laptop)
            .FirstOrDefault(d => d.MaDonHang == id);

        if (donHang == null) return NotFound();

        return View("InHoaDon", donHang); // View dùng layout riêng (in)
    }
    [HttpPost]
    public async Task<IActionResult> XacNhan(string DiaChiGiaoHang, string DienThoaiGiaoHang)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");


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
            DiaChiGiaoHang = DiaChiGiaoHang,
            DienThoaiGiaoHang = DienThoaiGiaoHang
        };
        _context.DonHang.Add(donHang);
        await _context.SaveChangesAsync();

        // Thêm chi tiết đơn hàng
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

        // Xóa giỏ hàng sau khi thanh toán
        _context.ChiTietGioHang.RemoveRange(gioHang);
        await _context.SaveChangesAsync();

        // Chuyển về trang quản lý đơn hàng (có thể đổi sang ThankYou nếu cần)
        return Redirect("/Admin/DonHang/DanhSach");
    }



}
