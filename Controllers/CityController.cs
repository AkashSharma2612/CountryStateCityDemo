using Country_city_state.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Country_city_state.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class CityController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
       // private readonly IDataProtector _dataprotector;

        public CityController(ApplicationDbcontext context/*, IDataProtectionProvider dataProtectionProvider, SecurityPurpose securityPurpose*/)
        {
            _context = context;
          // _dataprotector = dataProtectionProvider.CreateProtector(securityPurpose.key);
        }
        // [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCity()
        {
                var citiesList = _context.Cities.Include(c => c.State).Include(c => c.State.Country).ToList().Select(e => new
                {
                id = e.id,
                name = SecurityPurpose.DecryptionData(e.Name),
                stateid = e.Stateid,
                statename = SecurityPurpose.DecryptionData(e.State.Name),
                countryid = e.State.Countryid,
                countryname = SecurityPurpose.DecryptionData(e.State.Country.Name),

            });
           

            return Ok(citiesList);

        }
      
        [HttpPost]
        public IActionResult SaveCities([FromBody]City city)
        {
            if (city == null) return BadRequest();
            else
            {
               city.Name = SecurityPurpose.Encryption(city.Name);
               // city.Name = _dataprotector.Protect(city.Name);
                _context.Cities.Add(city);
                _context.SaveChanges();
            }
            return Ok(new { message = "data saved" });
        }
        [HttpPut]
        public IActionResult UpdateCity( [FromBody] City  city)
        {
            if (city == null)
                return BadRequest();
            else
            {
               
                _context.Cities.Update(city);
                _context.SaveChanges();

            }
            return Ok(new { message = "data updated" });
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCity(int id)
        {
            var cityindb = _context.Cities.Find(id);
            _context.Cities.Remove(cityindb);
            _context.SaveChanges();
            return Ok(new { messsage = "data deleted" });

        }
    }
}
