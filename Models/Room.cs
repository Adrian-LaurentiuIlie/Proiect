namespace Proiect.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        //public int? HotelID {  get; set; }
        public int Number {  get; set; }
        public int Floor {  get; set; }
        public string Type {  get; set; }
        public decimal Price {  get; set; }

        public Hotel? Hotel { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
