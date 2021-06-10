using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Models;

namespace Book.Services
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Authenricate(string username, string password);
        User Register(string username, string password);
    }
}
