using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourismMVC.Data;
using TourismMVC.Models;

namespace TourismMVC.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BookingsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Booking.Include(b => b.Attraction).Include(b => b.User);

            decimal totalBalance = await applicationDbContext.Where(b => b.Status != "Paid").Select(b => b.Balance).SumAsync();
            bool pendingBooking = await applicationDbContext.Where(b => b.Status == "Pending").AnyAsync();

            ViewBag.TotalBalance = totalBalance;
            ViewBag.PendingBooking = pendingBooking;

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Attraction)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("ConfirmPayment")] 
        public async Task<IActionResult> ConfirmPayment()
        {
            var bookings = await _context.Booking.Where(b => b.Status == "Pending").ToListAsync();

            if (!bookings.Any())
            {
                return NotFound("No bookings found.");
            }

            foreach (var booking in bookings)
            {
                booking.Status = "Paid"; 
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
