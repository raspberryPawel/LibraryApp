using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using LibApp.Dtos;
using System.Threading.Tasks;
using AutoMapper;
using LibApp.Controllers.Base;

namespace LibApp.Controllers
{
    public class CustomersController : BaseController
    {
        public CustomersController(ApplicationDbContext contex, IMapper mapper) : base(contex, mapper) {
        }

        public ViewResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id) => View(await this.MakeGetRequest<CustomerDto>($"customers/{id}"));

        public IActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null) return NotFound();

            var viewModel = new CustomerFormViewModel(customer)
            {
                MembershipTypes = _context.MembershipTypes.ToList(),
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Customer customer)
        {
            if (customer.Id == 0) await this.MakePostRequest<Customer>($"customers", customer);
            else await this.MakePutRequest<CustomerDto>($"customers/{_context.Customers.Single(c => c.Id == customer.Id).Id}", _mapper.Map<CustomerDto>(customer));

            return RedirectToAction("Index", "Customers");
        }
    }
}