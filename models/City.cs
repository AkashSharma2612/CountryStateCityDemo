using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Country_city_state.models
{
    public class City
    {
        public int id { get; set; }

        public string Name { get; set; }

        public int Stateid { get; set; }
      
        public State State { get; set; }
    }
}
