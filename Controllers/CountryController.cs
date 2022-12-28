using Country_city_state.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country_city_state.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
     //   private readonly IDataProtector _dataprotector;

        public CountryController(ApplicationDbcontext context/*IDataProtectionProvider dataProtectionProvider ,SecurityPurpose securityPurpose*/)
        {
            _context = context;
          // _dataprotector = dataProtectionProvider.CreateProtector(securityPurpose.key);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCountry()
        {
            var countryList = _context.Countries.ToList().Select(e =>new
             {
                 id = e.id,
                name = SecurityPurpose.DecrtyptionData(e.Name)
             });
           /* var Data = _context.Countries.Select(e => new
            {
                id = e.id,
                name = SecurityPurpose.DecrtyptionData(e.Name)

            });*/
            return Ok(countryList);
            
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SaveCountry([FromBody] Country country)
        {
            country.Name =SecurityPurpose.Encrtyption(country.Name);
           // country.id = Encrtyption(country.id.ToString());
            _context.Countries.Add(country);
            _context.SaveChanges();
            return Ok(new { message="Data saved"});
        }
        [HttpPut]
        public IActionResult UpdateCountry([FromBody] Country country)
        {

                _context.Countries.Update(country);
                _context.SaveChanges();
                return Ok(new { message = "data updated" });
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCountry(int id)
        {
            var countryindb = _context.Countries.Find(id);
            _context.Countries.Remove(countryindb);
            _context.SaveChanges();
            return Ok();

        }
       
    }
}
