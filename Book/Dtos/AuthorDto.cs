using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Models;

namespace Book.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }




        public virtual Country Country { get; set; }
        //public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
