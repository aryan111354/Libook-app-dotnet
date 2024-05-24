using project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Services
{
    //creating interface IReservationService 
    public interface IReservationService
    {
        // Specifying required methods of reservationservice class here 
        
        Task<bool> TestConnectionAsync();
        Task<Reservation> ReserveBookAsync(string userId, string bookId, DateTime reserveDate);

        Task<Reservation> GetReservationByCodeAndUserIdAsync(string reservationCode, string userId);

        Task<bool> UpdateReservationAsync(Reservation reservation);
    }
}
