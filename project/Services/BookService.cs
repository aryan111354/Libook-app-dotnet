using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace project.Services
{
    // creating a class book service which inherits Ibookservice 
    public class BookService : IBookService
    {
        // creating a string connection 
       string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=library;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
       
       // creating a method to access the books table in the database 
        public async Task<List<Books>> GetAllBooksAsync() 
        {
            var books = new List<Books>();
            
            
            // exeption to find if the database is accessed correctly or not, returning true of false values 
            try
            {
                // crating new connection to database 
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                //await connection.OpenAsync();

                // passing required query into the database using connection
                using (SqlCommand command = new SqlCommand("SELECT * FROM Books", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // retrieving the data from the database into list
                                var book = new Books
                                {
                                    BookId = reader["BookId"].ToString(),
                                    BookName = reader["BookName"].ToString(),
                                    IFSCcode = reader["IFSCcode"].ToString(),
                                    BookAuthor = reader["BookAuthor"].ToString(),
                                    DaysToRead = (int)reader["DaysToRead"]
                                };

                                books.Add(book);
                            }
                        }
                    }
                }
            
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllBooksAsync: " + ex.Message);
            }

            return books;
        }

        // created method for exeption to check connection of the database  with the code
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
