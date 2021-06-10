using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book.Services
{
    public interface IBookRepository
    {
        ICollection<Models.Book> GetBooks();
        Models.Book GetBook(int bookId);
        Models.Book GetBook(string bookIsbn);
        decimal GetBookRating(int bookId);
        bool BookExists(int bookId);
        bool BookExists(string bookIsbn);
        bool IsDuplicateIsbn(int bookId, string bookIsbn);

        bool CreateBook(List<int> authorsId, List<int> categoriesId, Models.Book book);
        bool UpdateBook(List<int> authorsId, List<int> categoriesId, Models.Book book);
        bool DeleteBook(Models.Book book);
        bool Save();
    }
}
