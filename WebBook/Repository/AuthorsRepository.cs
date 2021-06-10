using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebBook.Models;
using WebBook.Repository.IRepository;

namespace WebBook.Repository
{
    public class AuthorsRepository : Repository<Authors>, IAuthorsRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthorsRepository( IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
