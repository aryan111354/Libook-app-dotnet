using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Data
{
    // creating a reservation class which has attributes necessary for reservation including user ID 
    public class Reservation
    {
        public string ReservationId { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public DateTime ReserveDate { get; set; }
        public DateTime DateTillAvailable { get; set; }
        public decimal Cost { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }  
    }
}
