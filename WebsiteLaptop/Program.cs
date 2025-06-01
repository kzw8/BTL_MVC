    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using WebsiteLaptop.Data;
    using WebsiteLaptop.Models;
    using WebsiteLaptop.Services;

    var builder = WebApplication.CreateBuilder(args);

    // 1. Đăng ký DI
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<LaptopSearchService>();


    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    });

    builder.Services.AddDbContext<WebsiteLaptopContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // 2. Cấu hình middleware
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

// 3. Định nghĩa route



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=TaiKhoan}/{action=DangNhap}/{id?}"
);

// ✅ Route mặc định → Danh sách laptop
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Laptop}/{action=DanhSach}/{id?}",
    defaults: new { area = "User" }
);



// ✅ Nếu có route riêng admin (tùy chọn)


// 4. Tạo tài khoản admin duy nhất nếu chưa có
using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<WebsiteLaptopContext>();

        // Nếu chưa có admin nào
        if (!context.NguoiDung.Any(u => u.VaiTro == "Admin"))
        {
            var hasher = new PasswordHasher<NguoiDung>();

            var admin = new NguoiDung
            {
                TenDangNhap = "admin",
                Email = "quangkhai0810zz@gmail.com",
                HoVaTen = "Lương Quang Khải",
                MatKhau = "", // sẽ được hash
                VaiTro = "Admin",
                DienThoai = "0849775119",
                DiaChi = "Văn phòng"
            };

            admin.MatKhau = hasher.HashPassword(admin, "admin123"); // ✅ bạn có thể đổi mật khẩu tại đây

            context.NguoiDung.Add(admin);
            context.SaveChanges();

            Console.WriteLine("✅ Đã tạo tài khoản admin mặc định.");
        }

        if (!context.DanhMuc.Any())
        {
            context.DanhMuc.AddRange(
                new DanhMuc { TenDanhMuc = "Văn phòng" },
                new DanhMuc { TenDanhMuc = "Gaming" },
                new DanhMuc { TenDanhMuc = "Ultrabook" },
                new DanhMuc { TenDanhMuc = "Đồ họa - kỹ thuật" },
                new DanhMuc { TenDanhMuc = "Sinh viên" },
                new DanhMuc { TenDanhMuc = "Cảm ứng" },
                new DanhMuc { TenDanhMuc = "Laptop AI" }

            );
            context.SaveChanges();

            Console.WriteLine("✅ Đã thêm danh mục mẫu.");
        }
    }


    app.Run();
