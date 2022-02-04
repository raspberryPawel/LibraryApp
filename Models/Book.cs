using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace LibApp.Models
{
	public class Book
    {
        public int Id { get; set; }
		[Required(ErrorMessage = "Please enter a book name")]
		[StringLength(255, ErrorMessage = "Name must be shorten then 255 characters")]
		[Display(Name = "Book name:")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please enter a book author")]
		[Display(Name = "Autor name and surname (or alias):")]
		public string AuthorName { get; set; }
		[Required(ErrorMessage = "Please select genre")]
		public Genre Genre { get; set; }
		[Required(ErrorMessage = "Please select genre")]
		[Display(Name = "Book genre:")]
		public byte GenreId { get; set; }
		public DateTime DateAdded { get; set; }
		[Display(Name="Realease Date:")]
		[Required(ErrorMessage = "Please enter a book release date")]
		public DateTime ReleaseDate { get; set; }
		[Required(ErrorMessage = "Please enter number in stock value")]
		[Display(Name = "Number of books in stock:")]
		[Range(1, 20, ErrorMessage = "Number must be in range 1-20")]
		public int NumberInStock { get; set; }
		public int NumberAvailable { get; set; }
	}
      
}
