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
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<RoomStaff> RoomStaffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>().ToTable("Guest");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Hotel>().ToTable("Hotel");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<RoomStaff>().ToTable("RoomStaff");

            modelBuilder.Entity<RoomStaff>().HasKey(c => new { c.RoomID, c.StaffID });
        }
    }
}
