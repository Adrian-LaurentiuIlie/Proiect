using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models
{
    public class RoomStaff
    {
        public int RoomID { get; set; }
        public int StaffID { get; set; }
        public Room? Room { get; set; }
        public Staff? Staff { get; set; }
    }
}
