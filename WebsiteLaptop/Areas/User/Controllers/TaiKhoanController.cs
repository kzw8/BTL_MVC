using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

namespace WebsiteLaptop.Areas.User.Controllers
{
    [Area("User")]
    public class TaiKhoanController : Controller
    {
        private readonly WebsiteLaptopContext _context;
        public TaiKhoanController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DangNhap(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("DanhSach", "Laptop", new { area = "Admin" });

                return RedirectToAction("DanhSach", "Laptop");
            }

            return View(new DangNhap { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(DangNhap vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TenDangNhap == vm.TenDangNhap);

            if (user == null || new PasswordHasher<NguoiDung>().VerifyHashedPassword(user, user.MatKhau, vm.MatKhau) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties { IsPersistent = vm.RememberMe };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            if (user.VaiTro == "Admin")
                return RedirectToAction("DanhSach", "Laptop", new { area = "Admin" });

            return RedirectToAction("DanhSach", "Laptop");
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("DanhSach", "Laptop", new { area = "Admin" });

                return RedirectToAction("DanhSach", "Laptop", new { area = "User" });
            }

            return View(new DangKy());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(DangKy vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Kiểm tra trùng tài khoản
            bool exists = await _context.NguoiDung
                .AnyAsync(u => u.TenDangNhap == vm.TenDangNhap || u.Email == vm.Email);

            if (exists)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc Email đã tồn tại.");
                return View(vm);
            }

            // Tạo mới người dùng
            var user = new NguoiDung
            {
                TenDangNhap = vm.TenDangNhap,
                HoVaTen = vm.HoVaTen,
                Email = vm.Email,
                DienThoai = vm.DienThoai,
                DiaChi = vm.DiaChi,
                VaiTro = "User"
            };

            // Băm mật khẩu
            var hasher = new PasswordHasher<NguoiDung>();
            user.MatKhau = hasher.HashPassword(user, vm.MatKhau);

            // Lưu vào CSDL
            _context.NguoiDung.Add(user);
            await _context.SaveChangesAsync();

            // Gửi thông báo toast
            TempData["Success"] = "Đăng ký thành công! Bạn có thể đăng nhập.";

            return RedirectToAction("DangNhap");
        }


        [HttpGet]
        public IActionResult DangXuat()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("DanhSach", "Laptop");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuatXacNhan()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("DanhSach", "Laptop");
        }

        [HttpGet]
        public IActionResult TuChoiTruyCap()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> KiemTraEmailTrung(string email)
        {
            bool exists = await _context.NguoiDung.AnyAsync(u => u.Email == email);
            return Json(!exists);
        }
    }
}
