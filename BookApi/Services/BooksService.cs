using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Contracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookApi.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAll() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetById(string id) =>
            await _booksCollection.Find(x => x.BookId == id).FirstOrDefaultAsync();

        public async Task<Book?> GetByIdAndAuthor(string id, string author) =>
         await _booksCollection.Find(x => x.BookId == id && x.Author == author).FirstOrDefaultAsync();

        public async Task CreateBook(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateBook(string id, Book updatedBook) =>
            await _booksCollection.FindOneAndUpdateAsync(
            Builders<Book>.Filter.Where(x => x.BookId == id),
            Builders<Book>.Update
              .Set(book => book.Name, updatedBook.Name)
              .Set(book => book.Author, updatedBook.Author)
              .Set(book => book.Type, updatedBook.Type));

        public async Task RemoveBook(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.BookId == id);
    }
}
