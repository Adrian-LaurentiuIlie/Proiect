using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models.LibraryViewModels
{
    public class StaffIndexData
    {
        public IEnumerable<Staff> Staffs { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
