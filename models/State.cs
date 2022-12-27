using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Country_city_state.models
{
    public class State
    {
        public int id { get; set; }

        public string  Name { get; set; }

        public int Countryid { get; set; }
        [ForeignKey("Countryid")]
        public Country Country { get; set; }
    }
}
