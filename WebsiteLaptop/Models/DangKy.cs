using System.ComponentModel.DataAnnotations;
namespace WebsiteLaptop.Models
{
    public class DangKy
    {
        [Required, StringLength(50), Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = null!;

        [Required, DataType(DataType.Password), Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = null!;

        [Required, DataType(DataType.Password), Compare("MatKhau"),
         Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; } = null!;

        [StringLength(100), Display(Name = "Họ và tên")]
        public string? HoVaTen { get; set; }

        [Required, EmailAddress, StringLength(100), Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone, StringLength(20), Display(Name = "Điện thoại")]
        public string? DienThoai { get; set; }

        [StringLength(200), Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }
    }
}
