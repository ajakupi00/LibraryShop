using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace RWA_Library.DAL
{
    public class AuthorManager : IRepo<Author>
    {
        private string CS;

        public AuthorManager(string connectionString)
        {
            CS = connectionString;
        }
        public void Create(Author author, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Create) + nameof(Author), author.FullName, author.Description, author.PicturePath);
        }

        public void Delete(Author author)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Delete) + nameof(Author),  author.IDAuthor);
        }

        public IEnumerable<Author> GetAll()
        {
            var tblAuthor = SqlHelper.ExecuteDataset(CS, nameof(GetAll) + nameof(Author)).Tables[0];
            if (tblAuthor == null) return null;
            IList<Author> result = new List<Author>();
            foreach (DataRow row in tblAuthor.Rows)
            {
                result.Add(new Author
                {
                    IDAuthor = (int)row[nameof(Author.IDAuthor)],
                    FullName = row[nameof(Author.FullName)].ToString(),
                    PicturePath = row[nameof(Author.PicturePath)].ToString(),
                    Description = row[nameof(Author.Description)].ToString(),
                });
            }
            return result;
        }

        public Author Get(int id)
        {
            var tblAuthor = SqlHelper.ExecuteDataset(CS, nameof(Get) + nameof(Author), id).Tables[0];
            if (tblAuthor == null) return null;
            DataRow row = tblAuthor.Rows[0];

            return new Author
            {
                IDAuthor = id,
                FullName = row[nameof(Author.FullName)].ToString(),
                PicturePath = row[nameof(Author.PicturePath)].ToString(),
                Description = row[nameof(Author.Description)].ToString(),
            };
        }

        public void Update(Author author, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Update) + nameof(Author), author.IDAuthor, author.FullName, author.Description, author.PicturePath);
        }
    }
}
