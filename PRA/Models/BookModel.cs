using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;



namespace PRA.Models
{
    public class BookModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string PicturePath { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal LoanPrice { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Description { get; set; }



        public static BookModel Parse(Book book)
        {
            return new BookModel
            {
                Title = book.Title,
                PicturePath = book.PicturePath,
                Price = book.Price,
                LoanPrice = book.LoanPrice,
                Quantity = book.Quantity,
                ISBN = book.ISBN,
                Description = book.Description
            };
        }
        public static Book ParseFromBookModel(BookModel book, int id, Author author, Status status)
        {
            return new Book
            {
                Title = book.Title,
                PicturePath = book.PicturePath,
                Price = book.Price,
                LoanPrice = book.LoanPrice,
                Quantity = book.Quantity,
                ISBN = book.ISBN,
                Description = book.Description,
                Author = author,
                Status = status,
                IDBook = id
            };
        }

        public static Book ParseFromBookModel(BookModel book, Author author, Status status)
        {
            return new Book
            {
                Title = book.Title,
                PicturePath = book.PicturePath,
                Price = book.Price,
                LoanPrice = book.LoanPrice,
                Quantity = book.Quantity,
                ISBN = book.ISBN,
                Description = book.Description,
                Author = author,
                Status = status,
            };
        }



    }
}

