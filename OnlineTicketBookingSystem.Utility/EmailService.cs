
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace OnlineTicketBookingSystem.Utility
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "luanhotest@gmail.com"; // Thay bằng email gốc
        private readonly string _smtpPass = "afuasdwjsniwevmo"; // Thay bằng mật khẩu email 
        public async Task SendVerificationEmailAsync(string toEmail, string fullName, string subject, string codeId)
        {
            // Gắn HTML trực tiếp vào body
            string body = $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
                <meta charset='UTF-8' />
                <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                <meta http-equiv='X-UA-Compatible' content='ie=edge' />
                <meta name='description' content='Xác thực email cho tài khoản tại ThanhLuan. Nhận mã xác thực ngay để hoàn tất đăng ký.' />
                <title>{subject}</title>
                <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet' />
                <link href='https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap' rel='stylesheet' />
                <style>
                    body {{
                        font-family: 'Poppins', sans-serif;
                        background: #f4f7ff;
                        color: #434343;
                        margin: 0;
                        padding: 0;
                        width: 100%;
                        height: 100%;
                    }}
                    .container {{
                        max-width: 680px;
                        margin: 0 auto;
                        padding: 45px 30px 60px;
                        background: #ffffff;
                        border-radius: 10px;
                        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
                        text-align: center;
                        margin: 0 auto;
                    }}
                    .otp-code {{
                        font-size: 36px;
                        font-weight: 600;
                        letter-spacing: 20px;
                        color: #ba3d4f;
                        margin: 40px 0;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class='container my-5'>
                    <header class='mb-4'>
                        <h1 class='h4'>Mã xác thực của bạn</h1>
                    </header>
                    <main>
                        <p>Xin chào, {fullName}</p>
                        <p>Cảm ơn bạn đã đăng ký tại ThanhLuan.</p>
                        <p>Vui lòng sử dụng mã xác thực dưới đây để hoàn tất việc đăng ký. Mã xác thực có giá trị trong <strong>30 giây</strong>. Đừng chia sẻ mã này với bất kỳ ai.</p>
                        <p class='otp-code'>{codeId}</p>
                        <div class='help-section mt-4'>
                            <p>Cần hỗ trợ? Liên hệ tại <a href='mailto:luanhotest@gmail.com'>luanhotest@gmail.com</a></p>
                        </div>
                    </main>
                    <footer class='mt-4 pt-4 border-top'>
                        <p class='mb-1'><strong>ThanhLuan Company</strong></p>
                        <p>Địa chỉ Phan Thiết, Bình Thuận</p>
                        <div class='social-icons'>
                            <a href='#' class='me-2'>
                                <img src='https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661502815169_682499/email-template-icon-facebook' alt='Facebook' width='30' />
                            </a>
                            <a href='#' class='me-2'>
                                <img src='https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661504218208_684135/email-template-icon-instagram' alt='Instagram' width='30' />
                            </a>
                            <a href='#' class='me-2'>
                                <img src='https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661503043040_372004/email-template-icon-twitter' alt='Twitter' width='30' />
                            </a>
                            <a href='#'>
                                <img src='https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661503195931_210869/email-template-icon-youtube' alt='YouTube' width='30' />
                            </a>
                        </div>
                        <p class='mt-2'>&copy; 2024 Công ty. Bảo lưu mọi quyền.</p>
                    </footer>
                </div>
                <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js'></script>
            </body>
            </html>";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Thanh Luân", _smtpUser));
            message.To.Add(new MailboxAddress(fullName, toEmail));
            message.Subject = subject;
            // Đặt nội dung email từ HTML
            message.Body = new TextPart("html")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                // Kết nối với SMTP server
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}