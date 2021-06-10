using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook
{
    public static class SD
    {
        public static string APIBaseUrl = "http://localhost:48715/";
        public static string AuthorsAPIPath = APIBaseUrl + "api/v1/authors/";
        public static string BookAPIPath = APIBaseUrl + "api/v{version:apiVersion}/books/";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/Users/";
    }
}
