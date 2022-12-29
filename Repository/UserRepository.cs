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
        public User Authenticate(string Username, string Password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == Username);
            if (userInDb != null)
            {
                var userExist = SecurityPurpose.DecryptionData(userInDb.Password);
                userInDb.Password = userExist;
            }
            if (userInDb.UserName == Username && userInDb.Password == Password)
            {
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
            return null;
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
            user.Password = SecurityPurpose.Encryption(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }



    }
}
