using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public int? HotelID { get; set; }
        [DisplayName("Room Number")]
        public int Number { get; set; }
        public int Floor { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }

        public Hotel? Hotel { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<RoomStaff>? RoomStaffs { get; set; }
    }
}
