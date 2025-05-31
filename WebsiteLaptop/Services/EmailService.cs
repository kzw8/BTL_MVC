using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    public async Task GuiEmailXacNhan(string emailNguoiNhan, string hoTen)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("LaptopKZW", "youremail@gmail.com")); // đổi thành email gửi
        message.To.Add(new MailboxAddress(hoTen, emailNguoiNhan));
        message.Subject = "Xác nhận đăng ký tài khoản";

        message.Body = new TextPart("html")
        {
            Text = $"<p>Xin chào <b>{hoTen}</b>,</p><p>Cảm ơn bạn đã đăng ký tài khoản tại LaptopKZW.</p><p>Chúc bạn mua sắm vui vẻ!</p>"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, false); // SMTP của Gmail
        await client.AuthenticateAsync("youremail@gmail.com", "your-app-password"); // thay bằng email thật và mật khẩu ứng dụng
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
