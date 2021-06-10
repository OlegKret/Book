using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook.Models
{
    public class Country
    {
        
        public int Id { get; set; }

        
        public string Name { get; set; }
        public virtual ICollection<Authors> Authors { get; set; }
    }
}
