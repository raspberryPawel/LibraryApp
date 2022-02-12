using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Repositories
{
    public class BookRepository : IBookRepository
    {
        protected ApplicationDbContext context;
        protected IMapper mapper;
        public BookRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IEnumerable<Book> GetAll()
        {
            return context.Books.Include(b => b.Genre);
        }
        public Book GetById(int id) => context.Books.Include(b => b.Genre).SingleOrDefault(c => c.Id == id);
        public void Add(Book book) => context.Books.Add(book);
        public void Update(BookDto book) => context.Books.Update(mapper.Map(book, GetById(book.Id)));
        public void Delete(int id) => context.Books.Remove(GetById(id));
        public void Save() => context.SaveChanges();
    }
}
