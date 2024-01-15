using Microsoft.EntityFrameworkCore;
using Proiect.Models;

namespace Proiect.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>().ToTable("Guest");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Hotel>().ToTable("Hotel");
        }
    }
}
