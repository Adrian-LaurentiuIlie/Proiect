using System.ComponentModel;

namespace Proiect.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }

        public DateTime BirthDate { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
