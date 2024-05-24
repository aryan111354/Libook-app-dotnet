using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.Data;
using Microsoft.Data.SqlClient;

namespace project.Services
{
    // creating interface IBookService 
    public interface IBookService
    {
        // spcifying required methods in booksservice class here  
        Task<bool> TestConnectionAsync();
        Task<List<Books>> GetAllBooksAsync();
    }
}
