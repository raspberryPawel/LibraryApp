using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        // GET /api/books/genres
        [HttpGet]
        [Route("genres")]
        public IActionResult GetGenres()
        {
            return Ok(_context.Genre.ToList().Select(_mapper.Map<Genre, GenreDto>));
        }

        // GET /api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books.Include(c => c.Genre).SingleOrDefaultAsync(c => c.Id == id);
            if (book == null) throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            return Ok(_mapper.Map<BookDto>(book));
        }

        // POST /api/books/
        [HttpPost]
        public BookDto CreateBook(BookDto bookDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            var book = _mapper.Map<Book>(bookDto);
            book.NumberAvailable = book.NumberInStock;

            _context.Books.Add(book);
            _context.SaveChanges();
            bookDto.Id = book.Id;

            return bookDto;
        }

        // PUT api/books/{id}
        [HttpPut("{id}")]
        public void UpdateBook(int id, BookDto bookDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            var bookInDb = _context.Books.SingleOrDefault(c => c.Id == id);
            if (bookInDb == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);


            _mapper.Map(bookDto, bookInDb);
            _context.SaveChanges();
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
