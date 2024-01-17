using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryContext(serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()))
            {
                if (context.Rooms.Any())
                {
                    return;
                }

                context.Rooms.AddRange(
                    new Room { Number = 21, Floor = 2, Type = "Single", Price = Decimal.Parse("200") },
                    new Room { Number = 31, Floor = 3, Type = "Double", Price = Decimal.Parse("300") },
                    new Room { Number = 51, Floor = 5, Type = "Presidential", Price = Decimal.Parse("500") },
                    new Room { Number = 41, Floor = 4, Type = "Delux", Price = Decimal.Parse("400") }
                    );
                context.SaveChanges();

                context.Hotels.AddRange(
                   new Hotel { Name = "Clayton Hotel Charlemont", Adress = "Charlemont Street, Dublin, Co. Dublin, D02 H9C1" },
                   new Hotel { Name = "The Metropole Hotel", Adress = "MacCurtain Street, Cork, Cork" }
                   );
                context.SaveChanges();

                context.Guests.AddRange(
                   new Guest { FirstName = "Emily", LastName = "Johnson", Adress = "123 Oak Street, Apt 4B, Cityville, State 56789", BirthDate = DateTime.Parse("1985-05-15") },
                   new Guest { FirstName = "Alex", LastName = "Rodriguez", Adress = "789 Pine Avenue, Suite 22, Townsville, State 12345", BirthDate = DateTime.Parse("1990-07-08") },
                   new Guest { FirstName = "Olivia", LastName = "Martinez", Adress = "456 Maple Lane, Unit 7, Villagetown, State 98765", BirthDate = DateTime.Parse("1982-03-20") },
                   new Guest { FirstName = "Ethan", LastName = "Williams", Adress = "321 Birch Road, #12, Hamletville, State 54321", BirthDate = DateTime.Parse("1995-11-12") }
                   );
                context.SaveChanges();

                context.Bookings.AddRange(
                   new Booking { GuestID = 5, RoomID = 7, CheckInDate = DateTime.Parse("2024-01-07"), CheckOutDate = DateTime.Parse("2024-01-10"), TotalPrice = Decimal.Parse("1500") },
                   new Booking { GuestID = 8, RoomID = 5, CheckInDate = DateTime.Parse("2024-03-12"), CheckOutDate = DateTime.Parse("2024-03-17"), TotalPrice = Decimal.Parse("1000") },
                   new Booking { GuestID = 7, RoomID = 8, CheckInDate = DateTime.Parse("2024-05-19"), CheckOutDate = DateTime.Parse("2024-05-21"), TotalPrice = Decimal.Parse("800") },
                   new Booking { GuestID = 6, RoomID = 6, CheckInDate = DateTime.Parse("2024-02-22"), CheckOutDate = DateTime.Parse("2024-02-25"), TotalPrice = Decimal.Parse("900") }
                   );
                context.SaveChanges();

                var staffs = new Staff[]
                    {
                    new Staff { StaffName = "Anderson Emily", StaffJob = "Housekeper", StaffAdress = "123 Main Street, Anytown, USA", StaffPhoneNumber = "(555) 123-4567" },
                    new Staff { StaffName = "Smith Jacob", StaffJob = "Housekeper", StaffAdress = "456 Oak Avenue, Cityville, USA", StaffPhoneNumber = "(555) 987-6543" },
                    new Staff { StaffName = "Johnson Mia", StaffJob = "Housekeper", StaffAdress = "789 Pine Lane, Villagetown, USA", StaffPhoneNumber = "(555) 567-8901" },
                    new Staff { StaffName = "Williams Ethan", StaffJob = "Housekeper", StaffAdress = "321 Elm Street, Hamletville, USA", StaffPhoneNumber = "(555) 234-5678" },
                    };
                foreach (Staff s in staffs)
                {
                    context.Staffs.Add(s);
                }
                context.SaveChanges();

                var rooms = context.Rooms;
                context.RoomStaffs.AddRange(
                    new RoomStaff { RoomID = rooms.Single(r => r.Number == 51).RoomID, StaffID = staffs.Single(s => s.StaffName == "Anderson Emily").StaffID },
                    new RoomStaff { RoomID = rooms.Single(r => r.Number == 41).RoomID, StaffID = staffs.Single(s => s.StaffName == "Johnson Mia").StaffID },
                    new RoomStaff { RoomID = rooms.Single(r => r.Number == 21).RoomID, StaffID = staffs.Single(s => s.StaffName == "Smith Jacob").StaffID },
                    new RoomStaff { RoomID = rooms.Single(r => r.Number == 31).RoomID, StaffID = staffs.Single(s => s.StaffName == "Williams Ethan").StaffID }
                   );
                context.SaveChanges();


            }
        }
    }
}
