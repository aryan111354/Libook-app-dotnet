using Microsoft.Data.SqlClient;
using project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Services
{
    // creating reservationservice class which inherits the interface IReservationService
    public class ReservationService : IReservationService
    {
        
        // Declaring connection string 
        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=library;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        // creating method to access the reservation class 
        public async Task<Reservation> ReserveBookAsync(string userId, string bookId, DateTime reserveDate)
        {
            Reservation reservation = new Reservation();

            string reservationId = GenerateRandomReservationId(5);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlCommand = @"
            -- Declare variables
            DECLARE @CostPerDay DECIMAL(5, 2);
            DECLARE @DaysToRead INT;
            DECLARE @TotalCost DECIMAL(7, 2);
            DECLARE @DateTillAvailable DATE;

            -- Get cost per day and days to read from the Books table
            SELECT @CostPerDay = CostPerDay, @DaysToRead = DaysToRead
            FROM Books
            WHERE BookId = @BookId;

            -- Calculate total cost and date till available
            SET @TotalCost = @CostPerDay * @DaysToRead;
            SET @DateTillAvailable = DATEADD(day, @DaysToRead, @ReserveDate);

            -- Insert reservation into the Reservations table
            INSERT INTO Reservations (ReservationId, UserId, BookId, ReserveDate, DateTillAvailable, Cost)
            VALUES (@ReservationId, @UserId, @BookId, @ReserveDate, @DateTillAvailable, @TotalCost);

            -- Get the inserted reservation's details
            SELECT ReservationId, UserId, BookId, ReserveDate, DateTillAvailable, Cost
            FROM Reservations
            WHERE ReservationId = @ReservationId;";


                
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {

                    command.Parameters.AddWithValue("@ReservationId", reservationId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@ReserveDate", reserveDate);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            reservation.ReservationId = reader["ReservationId"].ToString();
                            reservation.UserId = reader["UserId"].ToString();
                            reservation.BookId = reader["BookId"].ToString();
                            reservation.ReserveDate = DateTime.Parse(reader["ReserveDate"].ToString());
                            reservation.DateTillAvailable = DateTime.Parse(reader["DateTillAvailable"].ToString());
                            reservation.Cost = decimal.Parse(reader["Cost"].ToString());
                        }
                    }
                }
            }

            return reservation;
        }

        // method to generato random resevation ID 
        private string GenerateRandomReservationId(int length)
        {

           
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        // accessing data using reservation code (reservationID) and user ID 
        public async Task<Reservation> GetReservationByCodeAndUserIdAsync(string reservationCode, string userId)
        {
            Reservation reservation = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlCommand = @"
            SELECT r.ReservationId, r.UserId, r.BookId, r.ReserveDate, r.DateTillAvailable, r.Cost,
                   b.BookName, b.BookAuthor
            FROM Reservations r
            INNER JOIN Books b ON r.BookId = b.BookId
            WHERE r.ReservationId = @ReservationCode AND r.UserId = @UserId;";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@ReservationCode", reservationCode);
                    command.Parameters.AddWithValue("@UserId", userId);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            reservation = new Reservation
                            {
                                ReservationId = reader["ReservationId"].ToString(),
                                UserId = reader["UserId"].ToString(),
                                BookId = reader["BookId"].ToString(),
                                ReserveDate = DateTime.Parse(reader["ReserveDate"].ToString()),
                                DateTillAvailable = DateTime.Parse(reader["DateTillAvailable"].ToString()),
                                Cost = decimal.Parse(reader["Cost"].ToString()),
                                BookName = reader["BookName"].ToString(),
                                Author = reader["BookAuthor"].ToString()
                            };
                        }
                    }
                }
            }

            return reservation;
        }

        // updating the reservation table in database on user  request
        public async Task<bool> UpdateReservationAsync(Reservation reservation)
{
    int rowsAffected = 0;

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string sqlCommand = @"
            UPDATE Reservations
            SET UserId = @UserId,
                BookId = @BookId,
                ReserveDate = @ReserveDate,
                DateTillAvailable = @DateTillAvailable,
                Cost = @Cost
            WHERE ReservationId = @ReservationId;";

        using (SqlCommand command = new SqlCommand(sqlCommand, connection))
        {
            command.Parameters.AddWithValue("@ReservationId", reservation.ReservationId);
            command.Parameters.AddWithValue("@UserId", reservation.UserId);
            command.Parameters.AddWithValue("@BookId", reservation.BookId);
            command.Parameters.AddWithValue("@ReserveDate", reservation.ReserveDate);
            command.Parameters.AddWithValue("@DateTillAvailable", reservation.DateTillAvailable);
            command.Parameters.AddWithValue("@Cost", reservation.Cost);

            await connection.OpenAsync();
            rowsAffected = await command.ExecuteNonQueryAsync();
        }
    }

    return rowsAffected > 0;
}

        //  exepiton for checking reservation table is accessed 
        public async Task<bool> TestConnectionAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }

}
