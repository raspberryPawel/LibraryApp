using LibApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Dtos
{
    public class RegisterCustomerDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter customer's name")]
        [StringLength(255)]
        public string Name { get; set; }
        public bool HasNewsletterSubscribed { get; set; } = false;
        [Required]
        public byte MembershipTypeId { get; set; } = 1;
        public DateTime? Birthdate { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
