using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourismMVC.Models;

namespace TourismMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TourismMVC.Models.Destination> Destination { get; set; } = default!;
        public DbSet<TourismMVC.Models.Attraction> Attraction { get; set; } = default!;
        public DbSet<TourismMVC.Models.Booking> Booking { get; set; } = default!;
    }
}
