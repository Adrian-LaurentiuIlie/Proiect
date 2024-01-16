using System.ComponentModel.DataAnnotations;

namespace Proiect.Models.LibraryViewModels
{
    public class BookingGroup
    {
        [DataType(DataType.Date)]
        public DateTime? BookingDate { get; set; }
        public int RoomCount {  get; set; }
    }
}
