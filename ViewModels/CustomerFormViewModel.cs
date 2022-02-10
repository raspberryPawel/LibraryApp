using LibApp.Dtos;
using LibApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibApp.ViewModels
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipTypeDto> MembershipTypes { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Please enter customer's name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter customer's email")]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }
        public bool HasNewsletterSubscribed { get; set; }
        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }
        [Display(Name = "Date of Birth")]
        [Min18YearsIfMember]
        public DateTime? Birthdate { get; set; }

        public string Title
        {
            get
            {
                return Id != "0" ? "Edit Customer" : "New Customer";
            }
        }

        public bool IsEditMode
        {
            get
            {
                return Id != "0";
            }
        }

        public CustomerFormViewModel()
        {
            Id = "0";
        }

        public CustomerFormViewModel(CustomerDto customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            HasNewsletterSubscribed = customer.HasNewsletterSubscribed;
            MembershipTypeId = customer.MembershipTypeId;
            Birthdate = customer.Birthdate;
        }
    }
}
