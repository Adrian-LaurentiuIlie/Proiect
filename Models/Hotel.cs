namespace Proiect.Models
{
    public class Hotel
    {
        public int HotelID {  get; set; }
        public string Name { get; set;}
        public string Adress { get; set;}
        public ICollection<Room>? Rooms { get; set; }
    }
}
