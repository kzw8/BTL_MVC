using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebsiteLaptop.Data;    // Namespace chứa lớp WebsiteLaptopContext

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 1.1 Đăng ký cookie auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// 1.2 Đăng ký policy hoặc role-based
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});



// 2. Thêm DbContext vào DI container
builder.Services.AddDbContext<WebsiteLaptopContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 3. Thêm MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Cấu hình middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // default 30 ngày
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 5. Định nghĩa route mặc định: Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Laptop}/{action=Index}/{id?}"
    );

app.Run();
builder.Services.AddHttpContextAccessor();
