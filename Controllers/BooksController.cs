using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using LibApp.Controllers.Base;
using AutoMapper;
using LibApp.Dtos;

namespace LibApp.Controllers
{
    public class BooksController : BaseController
    {
        public BooksController(ApplicationDbContext contex, IMapper mapper) : base(contex, mapper) { }

        public async Task<IActionResult> Index() => View(await this.MakeGetRequest<IEnumerable<BookDto>>($"books"));
        public async Task<IActionResult> Details(int id) => View(await this.MakeGetRequest<BookDto>($"books/{id}"));

        public async Task<IActionResult> Edit(int id)
        {
            var book = await this.MakeGetRequest<BookDto>($"books/{id}");
            if (book == null)return NotFound();
            
            var viewModel = new BookFormViewModel
            {
                Book = book,
                Genres = await this.MakeGetRequest<IEnumerable<GenreDto>>($"books/genres")
            };

            return View("BookForm", viewModel);
        }

        public async Task<IActionResult> New()
        {
            var viewModel = new BookFormViewModel
            {
                Genres = await this.MakeGetRequest<IEnumerable<GenreDto>>($"books/genres")
            };

            return View("BookForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Book book)
        {
            if (book.Id == 0) await this.MakePostRequest<Book>($"books", book);
            else await this.MakePutRequest<Book>($"books/{book.Id}", book);

            return RedirectToAction("Index", "Books");
        }
    }
}
