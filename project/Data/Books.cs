// Aryan Patel, Jay Patel, Navraj Singh, Sehajbir Singh 
// Group 9
// 2023-04-27
// This is the library management system where a user can find the book he/she likes and can reserve it for the days till when the book is available

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace project.Data
{
    // created books class having necessary attributes for a real book
    public class Books
    {
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string IFSCcode { get; set; }
        public int DaysToRead { get; set; }



    }
}
