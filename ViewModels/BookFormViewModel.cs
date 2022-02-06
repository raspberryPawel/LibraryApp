using LibApp.Dtos;
using LibApp.Models;
using System.Collections.Generic;

namespace LibApp.ViewModels
{
    public class BookFormViewModel
    {
        public IEnumerable<GenreDto> Genres { get; set; }
        public BookDto Book { get; set; }
        public string Title
        {
            get
            {
                if (Book != null && Book.Id != 0)
                {
                    return "Edit Book";
                }

                return "New Book";
            }
        }
    }
}
