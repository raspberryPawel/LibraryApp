using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using LibApp.Dtos;
using System.Threading.Tasks;
using AutoMapper;
using LibApp.Controllers.Base;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace LibApp.Controllers
{
    public class CustomersController : BaseController
    {

        protected UserManager<Customer> userManager;
        PasswordHasher<Customer> passwordHasher;

        public CustomersController(ApplicationDbContext contex, IMapper mapper, UserManager<Customer> userManager) : base(contex, mapper) {
            this.userManager = userManager;
            this.passwordHasher = new PasswordHasher<Customer>();
        }

        public ViewResult Index() => View();
        public async Task<IActionResult> Details(string id) => View(await this.MakeGetRequest<CustomerDto>($"customers/{id}"));

        public async Task<IActionResult> New()
        {
            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = await this.MakeGetRequest<IEnumerable<MembershipTypeDto>>($"customers/membershipTypes")
            };

            return View("CustomerForm", viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var customer = await this.MakeGetRequest<CustomerDto>($"customers/{id}");
            if (customer == null) return NotFound();

            var viewModel = new CustomerFormViewModel(customer)
            {
                MembershipTypes = await this.MakeGetRequest<IEnumerable<MembershipTypeDto>>($"customers/membershipTypes")
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CustomerDto customer)
        {
            if (customer.Id == "0") await this.MakePostRequest<CustomerDto>($"customers", customer);
            else await this.MakePutRequest<CustomerDto>($"customers/{customer.Id}", _mapper.Map<CustomerDto>(customer));

            return RedirectToAction("Index", "Customers");
        }
    }
}