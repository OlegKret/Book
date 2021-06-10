using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Models;

namespace Book.Services
{
    public interface IAuthorRepository
    {
        ICollection<Author> getAuthors();
        Author GetAuthor(int authorId);
        ICollection<Author> GetAuthorOfABook(int bookId);
        ICollection<Models.Book> GetBooksByAuthor(int authorId);
        bool AuthorExists(int authorId);

        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool Save();
    }
}
