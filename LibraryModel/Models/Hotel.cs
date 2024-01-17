using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models
{
    public class Hotel
    {
        public int HotelID { get; set; }
        [DisplayName("Hotel Name")]
        public string Name { get; set; }
        public string Adress { get; set; }
        public ICollection<Room>? Rooms { get; set; }
    }
}
