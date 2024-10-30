using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface ITransactionHistoryRepository : IRepository<TransactionHistory>
    {
        void Update(TransactionHistory transactionHistory);
    }
}
