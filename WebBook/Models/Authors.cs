using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook.Models
{
    public class Authors
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Country Country { get; set; }
        //public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
