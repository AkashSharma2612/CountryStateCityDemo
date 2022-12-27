using Country_city_state.models;
using Country_city_state.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Country_city_state.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IDataProtector _dataProtector;
        public UserController(IUserRepository userRepository, IDataProtectionProvider dataProtectionProvider, SecurityPurpose securityPurpose)
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector(securityPurpose.forsecurity);
        }
        [HttpGet]
        public IActionResult GetUsers()
        {

            var userInList = _userRepository.GetUsers().Select(e =>
            {
                e.UserName = _dataProtector.Protect(e.UserName);
                e.Password = _dataProtector.Protect(e.Password);
                /*e.UserName = _dataProtector.Unprotect(e.UserName);
                e.Password = _dataProtector.Unprotect(e.Password);*/
                // e.Role = _dataProtector.Unprotect(e.Role);
                e.Role = _dataProtector.Protect(e.Role);
                return e;

            });
            return Ok(userInList);

        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var IsUniqueUser = _userRepository.IsUniqueUser(user.UserName);
                if (!IsUniqueUser)
                    return BadRequest("Username In Use");
                var UserInfo = _userRepository.Register(user.UserName, user.Password);
                if (UserInfo == null)
                    return BadRequest();
            }
            return Ok();
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserVm userVM)
        {
            var user = _userRepository.Authenticate(userVM.UserName, userVM.Password);
            if (user == null)
                return BadRequest("Wrong Username / Password");
            return Ok(user);
        }


    }
}
