using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteLaptop.Data;
using WebsiteLaptop.Models;

namespace WebsiteLaptop.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebsiteLaptopContext _context;
        public AccountController(WebsiteLaptopContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new DangKy());
        }

        // POST: /Account/Register
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(DangKy vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Kiểm tra trùng tên đăng nhập hoặc email
            bool exists = await _context.NguoiDung
                .AnyAsync(u => u.TenDangNhap == vm.TenDangNhap
                            || u.Email == vm.Email);
            if (exists)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc email đã tồn tại");
                return View(vm);
            }

            // Tạo user mới
            var user = new NguoiDung
            {
                TenDangNhap = vm.TenDangNhap,
                MatKhau = vm.MatKhau,      // Chú ý: trong thực tế nên hash password!
                HoVaTen = vm.HoVaTen,
                Email = vm.Email,
                DienThoai = vm.DienThoai,
                DiaChi = vm.DiaChi,
                VaiTro = "User"           // Giá trị mặc định
            };
            _context.NguoiDung.Add(user);
            await _context.SaveChangesAsync();

            // Sau khi register, chuyển sang login
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new DangNhap { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(DangNhap vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Xác thực bằng bảng NguoiDung
            var user = await _context.NguoiDung
                .FirstOrDefaultAsync(u =>
                    u.TenDangNhap == vm.TenDangNhap
                 && u.MatKhau == vm.MatKhau);
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(vm);
            }

            // Tạo claims và đăng nhập bằng cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Name,           user.TenDangNhap),
                new Claim(ClaimTypes.Role,           user.VaiTro)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties { IsPersistent = vm.RememberMe };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                props
            );

            // Chuyển về URL gốc nếu có, ngược lại về Home/Index
            if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
