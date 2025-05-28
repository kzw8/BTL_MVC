using System.ComponentModel.DataAnnotations;
namespace WebsiteLaptop.Models
{
    public class DangNhap
    {
        [Required, Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = null!;

        [Required, DataType(DataType.Password), Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = null!;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
