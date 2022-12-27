using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Country_city_state.models
{
    public class ApplicationDbcontext:DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options):base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }
    }
}
