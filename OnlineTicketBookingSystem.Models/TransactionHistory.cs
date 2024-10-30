namespace OnlineTicketBookingSystem.Models
{
    public class TransactionHistory
    {
        public Guid Id { get; set; } // Khóa chính của giao dịch
        public Guid UserId { get; set; } // ID của người dùng liên quan đến giao dịch
        public User User { get; set; } // Thông tin người dùng
        public decimal Amount { get; set; } // Số tiền giao dịch
        public string Currency { get; set; } // Mã tiền tệ (VD: "vnd", "usd")
        public string TransactionId { get; set; } // ID giao dịch từ Stripe
        public string PaymentIntentId { get; set; } // ID của PaymentIntent từ Stripe
        public string Status { get; set; } // Trạng thái giao dịch (VD: "succeeded", "pending", "failed")
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Thời điểm tạo giao dịch
        public string PaymentMethod { get; set; } // Phương thức thanh toán (VD: "card", "bank_transfer")
        public string ReceiptUrl { get; set; } // URL hóa đơn giao dịch (nếu có)
        public string Description { get; set; } // Mô tả giao dịch (VD: "Nạp tiền vào tài khoản")
    }
}
