namespace OnlineTicketBookingSystem.Utility
{
    public static class SD
    {
        public const string Group_User_Admin = "Admin";
        public const string Group_User_Driver = "Driver";
        public const string Group_User_Customer = "Customer";

        public const string Gender_Male = "Male";
        public const string Gender_Female = "Female";
        public const string Gender_Other = "Other";

        public const string AccountType_Local = "Local";
        public const string AccountType_Social = "Social";

        public const string SeatStatus_Empty = "Empty";
        public const string SeatStatus_Sold = "Sold";
        public const string SeatStatus_Processing = "Processing";
        public const string SeatStatus_Maintenance = "Maintenance";

        public const string TripStatus_Scheduled = "Scheduled";
        public const string TripStatus_Departing = "Departing";
        public const string TripStatus_Completed = "Completed";
        public const string TripStatus_Cancelled = "Cancelled";
        public const string TripStatus_Expired = "Expired";

        public const string TripAssignmentStatus_Empty = "Empty";
        public const string TripAssignmentStatus_Pending = "Pending";
        public const string TripAssignmentStatus_Approved = "Approved";
        public const string TripAssignmentStatus_Complated = "Complated";
        public const string TripAssignmentStatus_Expired = "Expired";

        public const string TicketStatus_Pending = "Pending"; // chờ
        public const string TicketStatus_Confirmed = "Confirmed"; // đã xác nhận
        public const string TicketStatus_Completed = "Completed"; // đã hoàn thành
        public const string TicketStatus_Cancelled = "Cancelled"; // đã huỷ
        public const string TicketStatus_Refunded = "Refunded"; // đã hoàn tiền
        public const string TicketStatus_Expired = "Expired"; // đã hết hạn


        public const bool IsActive_False = false;
        public const bool IsActive_True = true;

        public const bool IsStatus_False = false;
        public const bool IsStatus_True = true;
    }
}
