using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;
using OnlineTicketBookingSystem.Utility;
using Stripe;
using Stripe.Checkout;

namespace AdminDriverDashboard.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class StripeController : Controller
    {
        private readonly StripeSettings _stripeSettings;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly JwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        public string SessionId { get; set; }

        public StripeController(IOptions<StripeSettings> stripeSettings, IWebHostEnvironment webHostEnvironment, JwtService jwtService, IUnitOfWork unitOfWork)
        {
            _stripeSettings = stripeSettings.Value;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        private int OriginalPrice(string amount)
        {
            int originalAmount = Convert.ToInt32(amount);
            double discount = 0.05;
            return Convert.ToInt32(originalAmount / (1 - discount));
        }
        private int ReducedPrice(string amount)
        {
            int originalAmount = Convert.ToInt32(amount);
            return originalAmount;
        }
        public IActionResult CreateCheckoutSession(string amount, string token)
        {
            if (amount == null)
            {
                TempData["Error"] = "Vui lòng nhập mệnh giá trước khi thanh toán";
                return RedirectToAction(nameof(Index));
            }
            var currency = "vnd"; // Currency code

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var originalPrice = OriginalPrice(amount);
            var claims = _jwtService.ValidateAndDecodeToken(token);
            claims.TryGetValue("email", out string email);

            var options = new SessionCreateOptions

            {

                PaymentMethodTypes = new List<string>
                {
                    "card"
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions

                    {

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = Convert.ToInt32(amount),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {

                                Name = "Nạp tiền: " + amount + "đ",
                                Description = "Mệnh giá: " + originalPrice + "đ",

                            }

                        },
                        Quantity = 1
                    }

                },

                Mode = "payment",
                SuccessUrl = $"https://localhost:7025/Guest/Stripe/success?token={token}&amount={amount}&sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "https://localhost:7025/Guest/Stripe",
                CustomerEmail = email,

            };

            var service = new SessionService();

            var session = service.Create(options);

            SessionId = session.Id;

            return Redirect(session.Url);

        }
        public async Task<IActionResult> success(string token, string amount, string sessionId)
        {
            var claims = _jwtService.ValidateAndDecodeToken(token);
            if (!claims.TryGetValue("nameid", out string nameid))
            {
                TempData["Error"] = "Lỗi không có người dùng";
                return View(nameof(CreateCheckoutSession));
            }

            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id.ToString() == nameid);
            if (user == null)
            {
                TempData["Error"] = "Lỗi không có người dùng";
                return View(nameof(CreateCheckoutSession));
            }

            // Khởi tạo SessionService để lấy thông tin phiên giao dịch Stripe
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId); // Lấy thông tin phiên giao dịch bằng SessionId đã lưu

            // Kiểm tra trạng thái thanh toán
            if (session.PaymentStatus == "paid")
            {
                // Kiểm tra xem giao dịch đã tồn tại chưa
                var existingTransaction = await _unitOfWork.TransactionHistory.GetFirstOrDefaultAsync(th => th.TransactionId == session.Id);
                if (existingTransaction != null)
                {
                    // Nếu đã tồn tại, không thực hiện cập nhật
                    TempData["Warning"] = "Giao dịch này đã được xử lý trước đó.";
                    return View();
                }

                // Lấy ID của PaymentIntent từ phiên giao dịch Stripe
                var paymentIntentId = session.PaymentIntentId;

                // Khởi tạo PaymentIntentService để lấy thông tin chi tiết thanh toán
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = paymentIntentService.Get(paymentIntentId);

                // Cập nhật số dư người dùng
                user.Balance += OriginalPrice(amount);
                _unitOfWork.User.Update(user);

                // Lưu lịch sử giao dịch (TransactionHistory) vào cơ sở dữ liệu
                var transactionHistory = new TransactionHistory
                {
                    UserId = user.Id,
                    OriginalPrice = OriginalPrice(amount),
                    Amount = ReducedPrice(amount),
                    Currency = "vnd", // Hoặc mã tiền tệ tương ứng
                    TransactionId = session.Id, // ID giao dịch từ Stripe
                    Status = session.PaymentStatus, // Trạng thái thanh toán
                    PaymentIntentId = paymentIntentId, // ID của PaymentIntent
                    PaymentMethod = paymentIntent.PaymentMethodTypes.FirstOrDefault(), // Phương thức thanh toán
                    Description = "Thanh toán nạp tiền " + amount + "đ" // Mô tả giao dịch
                };

                // Lấy thông tin charge và ReceiptUrl
                var chargeService = new ChargeService();
                var charges = chargeService.List(new ChargeListOptions
                {
                    PaymentIntent = paymentIntent.Id
                });

                var firstCharge = charges.Data.FirstOrDefault();
                if (firstCharge != null)
                {
                    transactionHistory.ReceiptUrl = firstCharge.ReceiptUrl; // Lấy URL hóa đơn (nếu có)
                }

                // Thêm giao dịch vào cơ sở dữ liệu
                await _unitOfWork.TransactionHistory.AddAsync(transactionHistory);
                await _unitOfWork.SaveAsync();

                // Truyền URL hóa đơn vào ViewData để hiển thị cho người dùng
                TempData["Success"] = "Nạp tiền thành công";
                ViewData["ReceiptUrl"] = transactionHistory.ReceiptUrl;
                ViewData["Amount"] = amount; // Thêm thông tin số tiền
                ViewData["OriginalPrice"] = OriginalPrice(amount);
                ViewData["TransactionId"] = transactionHistory.TransactionId; // Thêm thông tin ID giao dịch
            }
            else
            {
                TempData["Error"] = "Thanh toán không thành công.";
            }
            return View();
        }

        public IActionResult cancel()
        {
            return View(nameof(Index));
        }
    }
}
