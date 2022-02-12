using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.Repositories.Interfaces;
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
        protected PasswordHasher<Customer> passwordHasher;
        protected ICustomerRepository customerRepository;
        protected IMembershipTypeRepository membershipTypeRepository;

        public CustomersController(IMapper mapper, UserManager<Customer> userManager, ICustomerRepository customerRepository, IMembershipTypeRepository membershipTypeRepository)
        {
            _mapper = mapper;

            this.userManager = userManager;
            this.passwordHasher = new PasswordHasher<Customer>();
            this.customerRepository = customerRepository;
            this.membershipTypeRepository = membershipTypeRepository;
        }

        // GET /api/customers
        [HttpGet]
        public IActionResult GetCustomers(string query = null)
        {
            IEnumerable<Customer> customersQuery = customerRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(query))
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));

            var customerDtos = customersQuery
                .ToList()
                .Select(_mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }

        // GET /api/customers/{id}
        [HttpGet("{id}")]
        public IActionResult GetCustomer(string id)
        {
            Console.WriteLine("Request beginning");

            var customer = customerRepository.GetById(id);
            if (customer == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            Console.WriteLine("Request end");

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // GET /api/customers/membershipTypes
        [HttpGet]
        [Route("membershipTypes")]
        public IActionResult GetMembershipTypes()
        {
            
            return Ok(membershipTypeRepository.GetAll().Select(_mapper.Map<MembershipType, MembershipTypeDto>));
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
            if (customerRepository.GetById(id) == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            customerRepository.Delete(id);
            customerRepository.Save();
        }

        private readonly IMapper _mapper;
    }
}
