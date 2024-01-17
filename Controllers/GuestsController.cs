using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using LibraryModel.Models;

namespace Proiect.Controllers
{
    public class GuestsController : Controller
    {
        private readonly LibraryContext _context;

        public GuestsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Guests
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            int pageSize = 3;
            return View(await PaginatedList<Guest>.CreateAsync(_context.Guests.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .Include(s => s.Bookings)
                .ThenInclude(s => s.Room)
                .ThenInclude(s => s.Hotel)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // GET: Guests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Adress,BirthDate")] Guest guest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(guest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
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
            var guestToUpdate = await _context.Guests.FirstOrDefaultAsync(s => s.GuestID == id);
            if (await TryUpdateModelAsync<Guest>(guestToUpdate, "", s => s.FirstName, s => s.LastName, s => s.Adress, s => s.BirthDate))
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
            return View(guestToUpdate);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.GuestID == id);

            if (guest == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Guests == null)
            {
                return Problem("Entity set 'LibraryContext.Guests'  is null.");
            }
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool GuestExists(int id)
        {
          return (_context.Guests?.Any(e => e.GuestID == id)).GetValueOrDefault();
        }
    }
}
