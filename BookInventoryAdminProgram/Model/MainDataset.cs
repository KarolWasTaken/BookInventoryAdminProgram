using BookInventoryAdminProgram.Stores;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Dapper;

namespace BookInventoryAdminProgram.Model
{
    public class MainDataset
    {
        private List<MainDatasetStore> mainDatasetStore;

        public MainDataset(List<MainDatasetStore> mainDatasetStore)
        {
            this.mainDatasetStore = mainDatasetStore;
        }

        public class BookAuthor
        {
            public int BookID { get; set; }
            public string AuthorName { get; set; }
        }

        public class BookGenre
        {
            public int BookID { get; set; }
            public string GenreName { get; set; }
        }

        /// <summary>
        /// Gets data for MainDataset
        /// </summary>
        public void GetMainDataset()
        {
            List<BookAuthor> authorList;
            List<BookGenre> genreList;
            using (IDbConnection dbConnection = new SqlConnection(Helper.CnnVal()))
            {
                mainDatasetStore = dbConnection.Query<MainDatasetStore>("spGetDatabaseTestProcedure").ToList();
                authorList = dbConnection.Query<BookAuthor>("spGetBookAuthor").ToList();
                genreList = dbConnection.Query<BookGenre>("spGetBookGenre").ToList();
            }
            // here we turn the List<genre/author> into dictionaries for use later
            Dictionary<int, List<string>> bookAuthorDictionary = authorList
            .GroupBy(x => x.BookID)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.AuthorName).ToList()
            );
            Dictionary<int, List<string>> bookGenreDictionary = genreList
            .GroupBy(x => x.BookID)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.GenreName).ToList()
            );

            // here we combine out genre and author list into 
            foreach (var bookInfo in mainDatasetStore)
            {
                if (bookAuthorDictionary.ContainsKey(bookInfo.BookID))
                    bookInfo.Authors = bookAuthorDictionary[bookInfo.BookID];
                else if (bookGenreDictionary.ContainsKey(bookInfo.BookID))
                    bookInfo.Genres = bookGenreDictionary[bookInfo.BookID];
            }
        }
    }
}
