using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Proiect.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string StaffJob { get; set; }
        public string StaffAdress { get; set; }
        public string StaffPhoneNumber {  get; set; }

        public ICollection<RoomStaff>? RoomStaffs { get; set; }

    }
}
