using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.Repositories.Interfaces;
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
        protected IBookRepository repository;
        protected IGenreRepository genreRepository;
        public BooksController(IMapper mapper, IBookRepository bookRepository, IGenreRepository genreRepository)
        {
            this._mapper = mapper;
            this.repository = bookRepository;
            this.genreRepository = genreRepository;
        }

        // GET /api/books
        [HttpGet]
        public IActionResult GetBooks(string query = null) 
        {
            var booksQuery = repository.GetAll().Where(b => b.NumberAvailable > 0);

            if (!String.IsNullOrWhiteSpace(query))
                booksQuery = booksQuery.Where(b => b.Name.Contains(query));

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
            return Ok(genreRepository.GetAll().Select(_mapper.Map<Genre, GenreDto>));
        }

        // GET /api/books/{id}
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = repository.GetById(id);
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

            repository.Add(book);
            repository.Save();
            
            bookDto.Id = book.Id;
            return bookDto;
        }

        // PUT api/books/{id}
        [HttpPut("{id}")]
        public void UpdateBook(int id, BookDto bookDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            if (repository.GetById(id) == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            repository.Update(bookDto);
            repository.Save();
        }

        // DELETE /api/books
        [HttpDelete("{id}")]
        public void DeleteBook(int id)
        {
            var bookInDb = repository.GetById(id);
            if (bookInDb == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            repository.Delete(bookInDb.Id);
            repository.Save();
        }

        private readonly IMapper _mapper;
    }
}
