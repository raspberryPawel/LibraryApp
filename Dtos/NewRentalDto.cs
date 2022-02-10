using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Dtos
{
    public class NewRentalDto
    {
        public string CustomerId { get; set; }
        public List<int> BookIds { get; set; }
    }
}
