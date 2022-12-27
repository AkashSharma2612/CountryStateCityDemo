using Country_city_state.models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Country_city_state
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbcontext _context;
        private readonly Appsettings _appsettings;
        public UserRepository(ApplicationDbcontext context, IOptions<Appsettings> appSettings)
        {
            _context = context;
            _appsettings = appSettings.Value;

        }
        public User Authenticate(string Username, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == Username && u.Password == password);
            if (userInDb == null)
                return null;
            //JWT Autentication
            var TokenHandeler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,userInDb.Id.ToString()),
                    new Claim(ClaimTypes.Role,userInDb.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = TokenHandeler.CreateToken(tokenDescriptor);
            userInDb.Token = TokenHandeler.WriteToken(token);
            userInDb.Password = "";
            return userInDb;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool IsUniqueUser(string Username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == Username);
            if (user == null)
                return true;
            else
                return false;
        }

        public User Register(string Username, string Password)
        {
            User user = new User()
            {
                UserName = Username,
                Password = Password,
                Role = "Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

       

    }
}
