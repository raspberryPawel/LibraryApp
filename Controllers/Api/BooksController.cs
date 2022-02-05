using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/books
        [HttpGet]
        public IActionResult GetBooks(string query = null) 
        {
            var booksQuery = _context.Books
                .Include(b => b.Genre)
                .Where(b => b.NumberAvailable > 0);

            if (!String.IsNullOrWhiteSpace(query))
            {
                booksQuery = booksQuery.Where(b => b.Name.Contains(query));
            }

            var booksDtos = booksQuery
            .ToList()
            .Select(_mapper.Map<Book, BookDto>);

            return Ok(booksDtos);
        }

        // DELETE /api/books
        [HttpDelete("{id}")]
        public void DeleteBook(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(c => c.Id == id);
            if (bookInDb == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _context.Books.Remove(bookInDb);
            _context.SaveChanges();
        }

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
    }
}
