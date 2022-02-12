using AutoMapper;
using LibApp.Controllers.Base;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace LibApp.Controllers
{
    public class AuthController : BaseController {
        protected UserManager<Customer> userManager;
        protected PasswordHasher<Customer> passwordHasher;
        public AuthController(IMapper mapper, UserManager<Customer> userManager) : base(mapper) {
             this.userManager = userManager;
             this.passwordHasher = new PasswordHasher<Customer>();
        }

        public async Task<IActionResult> Register()
        {
            var viewModel = new RegisterCustomersViewModel()
            {
                membershipTypes = await this.MakeGetRequest<IEnumerable<MembershipTypeDto>>($"customers/membershipTypes")
            };

            return View("Register", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(RegisterCustomerDto customer)
        {
            var newCustomer = new Customer()
            {
                Name = customer.Name,
                HasNewsletterSubscribed = customer.HasNewsletterSubscribed,
                MembershipTypeId = customer.MembershipTypeId,
                Birthdate = customer.Birthdate,
                Email = customer.Email,
                NormalizedEmail = (customer.Email).Normalize(),
                PasswordHash = this.passwordHasher.HashPassword(null, customer.Password),
                UserName = customer.Email.Normalize(),
                NormalizedUserName = customer.Email.Normalize(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            try
            {
                await this.userManager.CreateAsync(newCustomer);
                await this.userManager.AddToRoleAsync(newCustomer, "user");
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }

            return Redirect("/");
        }
    }
}
