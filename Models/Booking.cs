namespace Proiect.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int GuestID { get; set; }
        public int RoomID { get; set; }
        public DateTime CheckInDate {  get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice {  get; set; }

        public Guest? Guest { get; set; }
        public Room? Room { get; set; }
    }
}
