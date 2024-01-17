using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models.LibraryViewModels
{
    public class BookingGroup
    {
        [DataType(DataType.Date)]
        public DateTime? BookingDate { get; set; }
        public int RoomCount { get; set; }
    }
}
