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

namespace LibApp.Controllers
{
    public class CustomersController : BaseController
    {
        public CustomersController(ApplicationDbContext contex, IMapper mapper) : base(contex, mapper) {}

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

        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Save(Customer customer)
        {
            if (customer.Id == "0") await this.MakePostRequest<Customer>($"customers", customer);
            else await this.MakePutRequest<CustomerDto>($"customers/{customer.Id}", _mapper.Map<CustomerDto>(customer));

            return RedirectToAction("Index", "Customers");
        }
    }
}