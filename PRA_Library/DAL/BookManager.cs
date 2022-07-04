using Microsoft.ApplicationBlocks.Data;
using RWA_Library.DAL.Interfaces;
using RWA_Library.Models;
using System.Collections.Generic;
using System.Data;
using WebApplication1.Models;
using Status = WebApplication1.Models.Status;

namespace RWA_Library.DAL
{
   
    public class BookManager : IBookManager
    {
        private string CS;
        public AuthorManager am;

        public BookManager(string connectionString)
        {
            CS = connectionString;
            am = new AuthorManager(connectionString);
        }
        public void Create(Book book, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Create) + nameof(book),
                book.Title, book.Author.IDAuthor, book.PicturePath, book.Price,
                book.LoanPrice, book.ISBN, book.Status.IdStatus,
                book.Quantity, book.Description);
        }

        public void Delete(Book book)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Delete), book.IDBook);
        }

        public Book Get(int id)
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(Get) + nameof(Book), id).Tables[0];
            if (tblBook == null) return null;
            DataRow row = tblBook.Rows[0];

            return new Book
            {
                IDBook = id,
                Title = row[nameof(Book.Title)].ToString(),
                Author = am.Get((int)row[nameof(Author.IDAuthor)]),
                PicturePath = row[nameof(Book.PicturePath)].ToString(),
                Price = (decimal)row[nameof(Book.Price)],
                LoanPrice = (decimal)row[nameof(Book.LoanPrice)],
                ISBN = (string)row[nameof(Book.ISBN)],
                Status = GetStatus((int)row[nameof(Book.Status.IdStatus)]),
                Quantity = (int)row[nameof(Book.Quantity)],
                Description = row[nameof(Book.Description)].ToString()
            };
        }

        public Status GetStatus(int id)
        {
            var tblStatus = SqlHelper.ExecuteDataset(CS, nameof(GetStatus), id).Tables[0];
            if (tblStatus == null) return null;
            DataRow row = tblStatus.Rows[0];
            return new Status
            {
                IdStatus = (int)row[nameof(Status.IdStatus)],
                StatusName = row[nameof(Status.StatusName)].ToString()
            };

        }

        public IEnumerable<Status> GetAllStatus()
        {
            var tblStatus = SqlHelper.ExecuteDataset(CS, nameof(GetAllStatus)).Tables[0];
            if (tblStatus == null) return null;
            IList<Status> result = new List<Status>();

            foreach (DataRow row in tblStatus.Rows)
            {
                result.Add(new Status
                {
                    IdStatus = (int)row[nameof(Status.IdStatus)],
                    StatusName = row[nameof(Status.StatusName)].ToString()
                });
            }
            return result;
        }

        public IEnumerable<Book> GetAll()
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(GetAll) + nameof(Book)).Tables[0];
            if (tblBook == null) return null;
            IList<Book> result = new List<Book>();



            foreach (DataRow row in tblBook.Rows)
            {
                result.Add(new Book
                {
                    Title = row[nameof(Book.Title)].ToString(),
                    Author = am.Get((int)row[nameof(Book.Author.IDAuthor)]),
                    PicturePath = row[nameof(Book.PicturePath)].ToString(),
                    Price = (decimal)row[nameof(Book.Price)],
                });
            }
            return result;
        }
        public IEnumerable<Book> GetAllBooksByReading(READING_FILTER filter)
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(GetAllBooksByReading), filter.ToString()).Tables[0];
            if (tblBook == null) return null;
            IList<Book> result = new List<Book>();



            foreach (DataRow row in tblBook.Rows)
            {
                result.Add(new Book
                {
                    Title = row[nameof(Book.Title)].ToString(),
                    Author = am.Get((int)row[nameof(Book.Author.IDAuthor)]),
                    PicturePath = row[nameof(Book.PicturePath)].ToString(),
                    Price = (decimal)row[nameof(Book.Price)],
                });
            }
            return result;
        }

        public void RemoveBook(int bookid)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(RemoveBook), bookid);
        }

        //ovo
        public void Update(Book book, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Update) + nameof(book),
                book.IDBook, book.Title, book.Author.IDAuthor, book.PicturePath,
                book.Price, book.LoanPrice, book.ISBN, book.Status.IdStatus,
                book.Quantity, book.Description);
        }


        public Book GetByTitle(string title)
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(GetByTitle), title).Tables[0];
            if (tblBook == null) return null;
            DataRow row = tblBook.Rows[0];



            int qnt = (int)row[nameof(Book.Quantity)];
            return new Book
            {
                IDBook = (int)row[nameof(Book.IDBook)],
                Title = row[nameof(Book.Title)].ToString(),
                Author = am.Get((int)row[nameof(Author.IDAuthor)]),
                PicturePath = row[nameof(Book.PicturePath)].ToString(),
                Price = (decimal)row[nameof(Book.Price)],
                LoanPrice = (decimal)row[nameof(Book.LoanPrice)],
                ISBN = (string)row[nameof(Book.ISBN)],
                Status = GetStatus((int)row[nameof(Book.Status.IdStatus)]),
                Quantity = qnt,
                Description = row[nameof(Book.Description)].ToString()
            };
        }

        public void UpdateToUsed(Book book, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(UpdateToUsed) + nameof(book),
            book.IDBook, book.Title, book.Author.IDAuthor, book.PicturePath,
            book.Price, book.LoanPrice, book.ISBN,
            book.Quantity, book.Description);
        }

        public int GetQntOfBook(string title, int statusid)
        {
            var tbl = SqlHelper.ExecuteDataset(CS, nameof(GetQntOfBook), title, statusid).Tables[0];
            if (tbl == null || tbl.Rows.Count == 0) return 0;
            DataRow row = tbl.Rows[0];
            return (int)row[nameof(Book.Quantity)];
        }
    }
}
