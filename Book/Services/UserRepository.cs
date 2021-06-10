using Book.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Book.Services
{
    public class UserRepository : IUserRepository
    {
        private BookDbContext _userDbContext;
        private AppSettings _appSettings;

        public UserRepository(BookDbContext userDbContext, IOptions<AppSettings> appsettings)
        {
            _userDbContext = userDbContext;
            _appSettings = appsettings.Value;
        }

        public User Authenricate(string username, string password)
        {
            var user = _userDbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            
            //user not found
            if (user == null)
            {
                return null;
            }

            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    //new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _userDbContext.Users.SingleOrDefault(x => x.Username == username);

            if (user == null)
                return true;

            return false;
        }

        [AllowAnonymous]
        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };
            _userDbContext.Users.Add(userObj);
            _userDbContext.SaveChanges();
            userObj.Password = "";
            return userObj;
        }
    }
}
