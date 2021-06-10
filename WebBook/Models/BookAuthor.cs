using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook.Models
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Authors Author { get; set; }
    }
}
