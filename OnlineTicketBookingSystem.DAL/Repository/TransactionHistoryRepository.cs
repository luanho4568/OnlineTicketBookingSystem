using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class TransactionHistoryRepository : Repository<TransactionHistory>, ITransactionHistoryRepository
    {
        private readonly ApplicationDbContext _db;
        public TransactionHistoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TransactionHistory transactionHistory)
        {
            _db.TransactionHistories.Update(transactionHistory);
        }
    }
}
