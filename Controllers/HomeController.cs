using Microsoft.AspNetCore.Mvc;
using Proiect.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibraryModel.Data;
using LibraryModel.Models.LibraryViewModels;

namespace Proiect.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;

        public HomeController(LibraryContext context) 
        { 
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> Statistics()
        {
            IQueryable<BookingGroup> data =
                from booking in _context.Bookings
                group booking by booking.CheckInDate into dateGroup
                select new BookingGroup()
                {
                    BookingDate = dateGroup.Key,
                    RoomCount = dateGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Chat() 
        { 
            return View(); 
        }

    }
}