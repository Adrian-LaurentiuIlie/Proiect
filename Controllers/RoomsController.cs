using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using LibraryModel.Models;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;

namespace Proiect.Controllers
{
    
    [Authorize(Policy = "Employee")]
    public class RoomsController : Controller
    {
        private readonly LibraryContext _context;

        public RoomsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Rooms
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewData["FloorSortParm"] = sortOrder == "Floor" ? "floor_desc" : "Floor";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null) 
            { 
                pageNumber = 1; 
            } else { 
                searchString = currentFilter; 
            }
            ViewData["TypeFilter"] = searchString;
            var rooms = from r in _context.Rooms
                        select r;
            if (!String.IsNullOrEmpty(searchString)) 
            { 
                rooms = rooms.Where(r => r.Type.Contains(searchString)); 
            }
            switch (sortOrder)
            {
                case "number_desc": rooms = rooms.OrderByDescending(r => r.Number); break;
                case "Floor": rooms = rooms.OrderBy(r => r.Floor); break;
                case "floor_desc": rooms = rooms.OrderByDescending(r => r.Floor); break;
                case "Price": rooms = rooms.OrderBy(r => r.Price); break;
                case "price_desc": rooms = rooms.OrderByDescending(r => r.Price); break;
                default:
                    rooms = rooms.OrderBy(r => r.Number);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Room>.CreateAsync(rooms.Include(s => s.Hotel).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(s => s.Hotel)
                .Include(s => s.Bookings)
                .ThenInclude(e => e.Guest)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var hotelList = _context.Hotels.Select(x => new { x.HotelID, x.Name});
            ViewData["HotelID"] = new SelectList(hotelList, "HotelID", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Number,Floor,Type,Price,HotelID")] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }

            ViewData["HotelID"] = new SelectList(_context.Hotels, "HotelID", "HotelID", room.HotelID);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var hotelList = _context.Hotels.Select(x => new { x.HotelID, x.Name });
            ViewData["HotelID"] = new SelectList(hotelList, "HotelID", "Name", room.HotelID);
            return View(room);
        }

        // POST: Rooms/Edit/5
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
            var roomToUpdate = await _context.Rooms.FirstOrDefaultAsync(s => s.RoomID == id);
            if (await TryUpdateModelAsync<Room>(roomToUpdate, "",s => s.Number, s => s.Floor, s => s.Type, s => s.Price, s => s.HotelID))
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
            return View(roomToUpdate);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault()) 
            { 
                ViewData["ErrorMessage"] = "Delete failed. Try again"; 
            }
            var hotelList = _context.Hotels.Select(x => new { x.HotelID, x.Name });
            ViewData["HotelID"] = new SelectList(hotelList, "HotelID", "Name", room.HotelID);
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'LibraryContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) 
            { 
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool RoomExists(int id)
        {
          return (_context.Rooms?.Any(e => e.RoomID == id)).GetValueOrDefault();
        }
    }
}
