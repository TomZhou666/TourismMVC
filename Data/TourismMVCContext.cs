using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourismMVC.Models;

namespace TourismMVC.Data
{
    public class TourismMVCContext : DbContext
    {
        public TourismMVCContext (DbContextOptions<TourismMVCContext> options)
            : base(options)
        {
        }

        public DbSet<TourismMVC.Models.Destination> Destination { get; set; } = default!;
        public DbSet<TourismMVC.Models.Attraction> Attraction { get; set; } = default!;
    }
}
