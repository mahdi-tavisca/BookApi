using BookApi.Contracts;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BookController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> getBooks()
        {
            var books = await _booksService.GetAll();
            return Ok(books);
        }

        [Route("getBook/{id}")]
        [HttpGet]
        public async Task<ActionResult<Book>> getBook(string id)
        {
            var book = await _booksService.GetById(id);
            if (book == null)
            {
                return NoContent();
            }
            return Ok(book);

        }

        [Route("getBook/{id}/{author}")]
        [HttpGet]
        public async Task<ActionResult<Book>> getBook(string id, string author)
        {

            string[] genres = { "Fantasy", "Scifi",
                        "Horror", "Adventure" };
            List<string> genreRange = new List<string>(genres);

            var book = await _booksService.GetByIdAndAuthor(id, author);
            if (book == null)
            {
                return NoContent();
            }
            else if (genreRange.Contains(book.Type))
            {
                return Ok(book);
            }
            else
            {
                var error = new
                {
                    error = "Book genre does not exist"
                };

                return BadRequest(error);
            }



        }

        [HttpPost]
        [Route("addbook")]
        public async Task<IActionResult> createBook(Book book)
        {
            await _booksService.CreateBook(book);
            return Ok(book);
        }

        [HttpPut]
        [Route("updatebook/{id}")]
        public async Task<IActionResult> updateBook(string id, Book updatedBook)
        {
            var queriedBook = await _booksService.GetById(id);
            if (queriedBook == null)
            {
                return NotFound();
            }
            await _booksService.UpdateBook(id, updatedBook);
            var message = new
            {
                message = "Book updated successfully"
            };
            return Ok(message);
        }

        [HttpDelete]
        [Route("deletebook/{id}")]
        public async Task<IActionResult> deleteBook(string id)
        {
            var queriedBook = await _booksService.GetById(id);
            if (queriedBook == null)
            {
                return NotFound();
            }
            await _booksService.RemoveBook(id);
            return NoContent();
        }
    }
}
