using Microsoft.AspNetCore.Mvc;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using X.PagedList.Extensions;

namespace AdminDriverDashboard.Areas.Guest.Controllers
{
    [Area("Guest")]

    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(Guid userId, int? page)
        {
            int pageSize = 10; // Số lượng vé mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1 nếu không có tham số 'page'

            // Lấy tất cả các vé của người dùng từ cơ sở dữ liệu
            await _unitOfWork.Trips.GetAllAsync(includeProperties: "Buses,StartProvince,EndProvince");
            var tickets = await _unitOfWork.Tickets.GetAllAsync(x => x.UserId == userId, includeProperties: "Trips,User,Seats");

            // Phân trang dữ liệu
            var pagedTickets = tickets.ToPagedList(pageNumber, pageSize);

            // Truyền userId vào ViewData để có thể sử dụng trong view
            ViewData["UserId"] = userId;

            // Trả về View với dữ liệu phân trang
            return View(pagedTickets);
        }
    }

}
