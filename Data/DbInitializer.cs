using Microsoft.EntityFrameworkCore;
using Proiect.Models;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proiect.Data
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
                    ) ;

                context.Hotels.AddRange(
                   new Hotel { Name = "Clayton Hotel Charlemont", Adress = "Charlemont Street, Dublin, Co. Dublin, D02 H9C1" },
                   new Hotel { Name = "The Metropole Hotel", Adress = "MacCurtain Street, Cork, Cork" }
                   );

                context.Guests.AddRange(
                   new Guest { FirstName = "Emily", LastName = "Johnson", Adress= "123 Oak Street, Apt 4B, Cityville, State 56789", BirthDate = DateTime.Parse("1985-05-15") },
                   new Guest { FirstName = "Alex", LastName = "Rodriguez", Adress = "789 Pine Avenue, Suite 22, Townsville, State 12345", BirthDate = DateTime.Parse("1990-07-08") },
                   new Guest { FirstName = "Olivia", LastName = "Martinez", Adress = "456 Maple Lane, Unit 7, Villagetown, State 98765", BirthDate = DateTime.Parse("1982-03-20") },
                   new Guest { FirstName = "Ethan", LastName = "Williams", Adress = "321 Birch Road, #12, Hamletville, State 54321", BirthDate = DateTime.Parse("1995-11-12") }
                   );

                context.Bookings.AddRange(
                   new Booking { GuestID = 2, RoomID = 3, CheckInDate = DateTime.Parse("2024-01-07"), CheckOutDate = DateTime.Parse("2024-01-10"), TotalPrice = Decimal.Parse("1500") },
                   new Booking { GuestID = 4, RoomID = 1, CheckInDate = DateTime.Parse("2024-03-12"), CheckOutDate = DateTime.Parse("2024-03-17"), TotalPrice = Decimal.Parse("1000") },
                   new Booking { GuestID = 1, RoomID = 4, CheckInDate = DateTime.Parse("2024-05-19"), CheckOutDate = DateTime.Parse("2024-05-21"), TotalPrice = Decimal.Parse("800") },
                   new Booking { GuestID = 3, RoomID = 2, CheckInDate = DateTime.Parse("2024-02-22"), CheckOutDate = DateTime.Parse("2024-02-25"), TotalPrice = Decimal.Parse("900") }
                   );

                context.SaveChanges();
            }
        }
    }
}
