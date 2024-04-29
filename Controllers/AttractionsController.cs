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
    public class AttractionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AttractionsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Attractions
        public async Task<IActionResult> Index(string attractionType, string searchString)
        {
            if(_context.Attraction == null)
            {
                return Problem("Entity set 'TourismMVCContext' is null.");
            }

            IQueryable<string> typeQuery = from a in _context.Attraction
                                           orderby a.Type
                                           select a.Type;

            var attractions = from a in _context.Attraction.Include(a => a.Destination)
                              select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                attractions = attractions.Where(a => a.Name!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(attractionType))
            {
                attractions = attractions.Where(a => a.Type == attractionType);
            }

            var attractionTypeVM = new AttractionTypeViewModel
            {
                Types = new SelectList(await typeQuery.Distinct().ToListAsync()),
                Attractions = await attractions.ToListAsync()
            };

            return View(attractionTypeVM);
        }

        // GET: Attractions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attraction = await _context.Attraction
                .Include(a => a.Destination)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }

            return View(attraction);
        }

        // GET: Attractions/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DestinationId"] = new SelectList(_context.Destination, "Id", "Name");
            return View();
        }

        // POST: Attractions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DestinationId,Name,Type,Address,ImageUrl,Description,Ticket")] Attraction attraction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attraction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinationId"] = new SelectList(_context.Destination, "Id", "Name", attraction.DestinationId);
            return View(attraction);
        }

        // GET: Attractions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attraction = await _context.Attraction.FindAsync(id);
            if (attraction == null)
            {
                return NotFound();
            }
            ViewData["DestinationId"] = new SelectList(_context.Destination, "Id", "Name", attraction.DestinationId);
            return View(attraction);
        }

        // POST: Attractions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DestinationId,Name,Type,Address,ImageUrl,Description,Ticket")] Attraction attraction)
        {
            if (id != attraction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attraction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionExists(attraction.Id))
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
            ViewData["DestinationId"] = new SelectList(_context.Destination, "Id", "Name", attraction.DestinationId);
            return View(attraction);
        }

        // GET: Attractions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attraction = await _context.Attraction
                .Include(a => a.Destination)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }

            return View(attraction);
        }

        // POST: Attractions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attraction = await _context.Attraction.FindAsync(id);
            if (attraction != null)
            {
                _context.Attraction.Remove(attraction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttractionExists(int id)
        {
            return _context.Attraction.Any(e => e.Id == id);
        }

        [HttpPost, ActionName("Book")]
        [Authorize]
        public async Task<IActionResult> Book(int id)
        {
            var attraction = await _context.Attraction.FirstOrDefaultAsync(a => a.Id == id);
            if (attraction == null)
            {
                return NotFound("Attraction not found.");
            }

            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound("User not found.");
            }

            var existingBooking = await _context.Booking
                                                .FirstOrDefaultAsync(b => b.UserId == userId && b.AttractionId == id);
            if (existingBooking != null)
            {
                _context.Booking.Update(existingBooking);
            }
            else
            {
                Booking newBooking = new Booking
                {
                    UserId = userId,
                    AttractionId = id,
                    Balance = (decimal)attraction.Ticket, 
                    CreatedDate = DateTime.Now,
                    Status = "Pending"
                };
                await _context.Booking.AddRangeAsync(newBooking);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); 
        }
    }
}
