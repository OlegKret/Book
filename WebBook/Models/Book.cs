using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbm { get; set; }
        public DateTime? DatePublished { get; set; }
    }
}
