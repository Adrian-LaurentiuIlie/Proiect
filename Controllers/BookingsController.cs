using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using LibraryModel.Models;

namespace Proiect.Controllers
{
    public class BookingsController : Controller
    {
        private readonly LibraryContext _context;

        public BookingsController(LibraryContext context)
        {
            _context = context;
        }

        public decimal CalculateTotalPrice(Room room, DateTime ChekInDate, DateTime ChekOutDate)
        {
            TimeSpan duration = ChekOutDate - ChekInDate;
            int numberOfNights = duration.Days;

            decimal totalPrice = numberOfNights * room.Price;
            return totalPrice;
        }
        // GET: Bookings
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CheckInDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "CheckInDate_desc" : "";
            ViewData["CheckOutDateSortParm"] = sortOrder == "CheckOutDate" ? "CheckOutDate_desc" : "CheckOutDate";
            ViewData["TotalPriceSortParm"] = sortOrder == "TotalPrice" ? "TotalPrice_desc" : "TotalPrice";

            var bookings = from b in _context.Bookings
                        select b;
            
            switch (sortOrder)
            {
                case "CheckInDate_desc": bookings = bookings.OrderByDescending(b => b.CheckInDate); break;
                case "CheckOutDate": bookings = bookings.OrderBy(b => b.CheckOutDate); break;
                case "CheckOutDate_desc": bookings = bookings.OrderByDescending(b => b.CheckOutDate); break;
                case "TotalPrice": bookings = bookings.OrderBy(b => b.TotalPrice); break;
                case "TotalPrice_desc": bookings = bookings.OrderByDescending(b => b.TotalPrice); break;
                default:
                    bookings = bookings.OrderBy(b => b.CheckInDate);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Booking>.CreateAsync(bookings.Include(b => b.Guest).Include(b => b.Room).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            var guestlList = _context.Guests.Select(x => new { x.GuestID, FullName = x.FirstName + " " + x.LastName });
            var roomList = _context.Rooms.Select(x => new { x.RoomID, x.Number });
            ViewData["GuestID"] = new SelectList(guestlList, "GuestID", "FullName");
            ViewData["RoomID"] = new SelectList(roomList, "RoomID", "Number");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestID,RoomID,CheckInDate,CheckOutDate,TotalPrice")] Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            ViewData["GuestID"] = new SelectList(_context.Guests, "GuestID", "GuestID", booking.GuestID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID", booking.RoomID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var guestlList = _context.Guests.Select(x => new { x.GuestID, FullName = x.FirstName + " " + x.LastName });
            var roomList = _context.Rooms.Select(x => new { x.RoomID, x.Number });
            ViewData["GuestID"] = new SelectList(guestlList, "GuestID", "FullName",booking.GuestID);
            ViewData["RoomID"] = new SelectList(roomList, "RoomID", "Number", booking.RoomID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookingToUpdate = await _context.Bookings.FirstOrDefaultAsync(s => s.BookingID == id);
            if (await TryUpdateModelAsync<Booking>(bookingToUpdate, "", s => s.GuestID, s => s.RoomID, s => s.CheckInDate, s => s.CheckOutDate, s => s.TotalPrice))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(bookingToUpdate);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }

            var guestlList = _context.Guests.Select(x => new { x.GuestID, FullName = x.FirstName + " " + x.LastName });
            var roomList = _context.Rooms.Select(x => new { x.RoomID, x.Number });
            ViewData["GuestID"] = new SelectList(guestlList, "GuestID", "FullName");
            ViewData["RoomID"] = new SelectList(roomList, "RoomID", "Number");
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'LibraryContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.BookingID == id)).GetValueOrDefault();
        }
    }
}
