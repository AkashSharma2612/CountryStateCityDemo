﻿using Country_city_state.models;
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
    public class StateController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
       // private readonly IDataProtector _dataprotector;
        public StateController(ApplicationDbcontext context/*, IDataProtectionProvider dataProtectionProvider, SecurityPurpose securityPurpose*/)
        {
            _context = context;
           // _dataprotector = dataProtectionProvider.CreateProtector(securityPurpose.forsecurity);

        }
       // [AllowAnonymous]
        [HttpGet]
        public IActionResult GetState()
        {
            var statesList = _context.States.Include(s => s.Country).ToList().Select(e => new
            {
                id = e.id,
                name = SecurityPurpose.DecryptionData(e.Name),
                countryid = e.Countryid,
                country = SecurityPurpose.DecryptionData(e.Country.Name)
            });
          

            return Ok(statesList);
        }
        
        [HttpPost]
        public IActionResult SaveState([FromBody] State state)
        {
            state.Name = SecurityPurpose.Encryption(state.Name);
            _context.States.Add(state);
            _context.SaveChanges();
            return Ok();
        }
       
        [HttpPut]
        public IActionResult UpdateState( [FromBody] State state)
        {
           // state.Name = _dataprotector.Protect(state.Name);
                _context.States.Update(state);
                _context.SaveChanges();
            return Ok(new { message = "data updated" });
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteState(int id)
        {
            var stateindb = _context.States.Find(id);
            _context.States.Remove(stateindb);
            _context.SaveChanges();
            return Ok(new { messsage = "data deleted" });

        }
    }
}
