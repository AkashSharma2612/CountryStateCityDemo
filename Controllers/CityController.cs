using Country_city_state.models;
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
    public class CityController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        private readonly IDataProtector _dataprotector;

        public CityController(ApplicationDbcontext context, IDataProtectionProvider dataProtectionProvider, SecurityPurpose securityPurpose)
        {
            _context = context;
            _dataprotector = dataProtectionProvider.CreateProtector(securityPurpose.forsecurity);
        }
        [HttpGet]
        public IActionResult GetCity()
        {
            var citiesList=_context.Cities.Include(c => c.State).Include(c => c.State.Country).ToList();
            var outData = _context.Cities.Select(e => new
            {
                id = _dataprotector.Protect(e.id.ToString()),
                name = _dataprotector.Protect(e.Name),
                StateID = _dataprotector.Protect(e.Stateid.ToString()),
                StateName=_dataprotector.Protect(e.State.Name),
                e.State.Country
            }); ;
            return Ok(outData);

        }
        [HttpPost]
        public IActionResult SaveCities([FromBody]City city)
        {
            if (city == null) return BadRequest();
            else
            {
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
