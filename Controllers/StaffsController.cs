using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using LibraryModel.Models;
using LibraryModel.Models.LibraryViewModels;

namespace Proiect.Controllers
{
    public class StaffsController : Controller
    {
        private readonly LibraryContext _context;

        public StaffsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Staffs
        public async Task<IActionResult> Index(int? id, int? roomID)
        {
            var viewModel = new StaffIndexData();
            viewModel.Staffs = await _context.Staffs
            .Include(i => i.RoomStaffs)
                .ThenInclude(i => i.Room)
                    .ThenInclude(i => i.Bookings)
                        .ThenInclude(i => i.Guest)
            .Include(i => i.RoomStaffs)
                .ThenInclude(i => i.Room)
                    .ThenInclude(i => i.Hotel)
            .AsNoTracking()
            .OrderBy(i => i.StaffName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["StaffID"] = id.Value;
                Staff staff = viewModel.Staffs.Where(
                i => i.StaffID == id.Value).Single();
                viewModel.Rooms = staff.RoomStaffs.Select(s => s.Room);
            }
            if (roomID != null)
            {
                ViewData["RoomID"] = roomID.Value;
                viewModel.Bookings = viewModel.Rooms.Where(
                x => x.RoomID == roomID).Single().Bookings;
            }
            return View(viewModel);
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(m => m.StaffID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffID,StaffName,StaffJob,StaffAdress,StaffPhoneNumber")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffID,StaffName,StaffJob,StaffAdress,StaffPhoneNumber")] Staff staff)
        {
            if (id != staff.StaffID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(m => m.StaffID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'LibraryContext.Staffs'  is null.");
            }
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
          return (_context.Staffs?.Any(e => e.StaffID == id)).GetValueOrDefault();
        }
    }
}
