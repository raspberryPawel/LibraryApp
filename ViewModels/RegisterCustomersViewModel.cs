using LibApp.Dtos;
using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.ViewModels
{
    public class RegisterCustomersViewModel
    {
        public RegisterCustomerDto customer { get; set;  }
        public IEnumerable<MembershipTypeDto> membershipTypes { get; set; }
    }
}
