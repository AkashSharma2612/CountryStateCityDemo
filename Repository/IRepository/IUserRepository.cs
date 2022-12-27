using Country_city_state.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Country_city_state
{
     public interface IUserRepository
    {
        ICollection<User> GetUsers();
        bool IsUniqueUser(string Username);
        User Authenticate(string Username, string password);
        User Register(string Username, string Password);

    }
}
