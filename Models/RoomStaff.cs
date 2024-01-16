namespace Proiect.Models
{
    public class RoomStaff
    {
        public int RoomID { get; set; }
        public int StaffID {  get; set; }
        public Room? Room { get; set; }
        public Staff? Staff { get; set; }
    }
}
