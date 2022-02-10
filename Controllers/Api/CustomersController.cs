using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        protected UserManager<Customer> userManager;
        PasswordHasher<Customer> passwordHasher;

        public CustomersController(ApplicationDbContext context, IMapper mapper, UserManager<Customer> userManager)
        {
            _context = context;
            _mapper = mapper;

            this.userManager = userManager;
            this.passwordHasher = new PasswordHasher<Customer>();
        }

        // GET /api/customers
        [HttpGet]
        public IActionResult GetCustomers(string query = null)
        {
            IEnumerable<Customer> customersQuery = _context.Customers
                .Include(c => c.MembershipType);

            if (!String.IsNullOrWhiteSpace(query))
            {
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));
            }

            var customerDtos = customersQuery
                .ToList()
                .Select(_mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }

        // GET /api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            Console.WriteLine("Request beginning");

            var customer = await _context.Customers.Include(c => c.MembershipType).SingleOrDefaultAsync(c => c.Id == id);
            //await Task.Delay(2000);
            if (customer == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            Console.WriteLine("Request end");

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // GET /api/customers/membershipTypes
        [HttpGet]
        [Route("membershipTypes")]
        public IActionResult GetMembershipTypes()
        {
            
            return Ok(_context.MembershipTypes.ToList().Select(_mapper.Map<MembershipType, MembershipTypeDto>));
        }

        // POST /api/customers/
        [HttpPost]
        public async Task<CustomerDto> CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }

            var newCustomer = new Customer()
            {
                Name = customerDto.Name,
                HasNewsletterSubscribed = customerDto.HasNewsletterSubscribed,
                MembershipTypeId = customerDto.MembershipTypeId,
                Birthdate = customerDto.Birthdate,
                Email = customerDto.Email,
                NormalizedEmail = (customerDto.Email).Normalize(),
                PasswordHash = this.passwordHasher.HashPassword(null, customerDto.Password),
                UserName = customerDto.Email.Normalize(),
                NormalizedUserName = customerDto.Email.Normalize(),
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

            return customerDto;
        }

        // PUT api/customers/{id}
        [HttpPut("{id}")]
        public async Task UpdateCustomer(string id, CustomerDto customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
                }

                var customer = await this.userManager.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }

                _mapper.Map(customerDto, customer);
                await this.userManager.UpdateAsync(customer);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        // DELETE /api/customers
        [HttpDelete("{id}")]
        public void DeleteCusomer(string id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
        }

        private ApplicationDbContext _context;
        private readonly IMapper _mapper;
    }
}
