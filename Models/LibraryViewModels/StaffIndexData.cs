using System.Security.Policy;

namespace Proiect.Models.LibraryViewModels
{
    public class StaffIndexData
    {
        public IEnumerable<Staff> Staffs { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
