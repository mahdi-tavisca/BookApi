using BookApi.Contracts;
using BookApi.Filters;
using BookApi.Services;
using BookApi.Validators;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [HttpExceptionFilter]
    public class BookController : ControllerBase
    {
        private readonly BooksService _booksService;
        private readonly AddBookRequestValidator _addBookRequestValidator;

        public BookController(BooksService booksService)
        {
            _booksService = booksService;
            _addBookRequestValidator = new AddBookRequestValidator();
        }

        [HttpGet]
        [HttpActionFilters]
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
                //return BadRequest(book);
                throw new Exception("Book not found"); // exception filter test
            }
            return Ok(book);

        }

        [Route("getBook/{id}/{author}")]
        [HttpGet]
        public async Task<ActionResult<Book>> getBook(string id, string author)
        {

            string[] genres = { "Romance",
                        "Horror", "Adventure"};
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
        [HttpAuthorizationFilter]
        [HttpResourceFilter]
        [Route("addbook")]
        public async Task<IActionResult> createBook(Book book)
        {
            var validatedResult = _addBookRequestValidator.Validate(book);
            if (validatedResult.IsValid)
            {
                await _booksService.CreateBook(book);
                return Ok(book);
            } else
            {
                List<string> errors = new List<string>();
                foreach(var error in validatedResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                var errorMessage = new
                {
                    errors
                };
                return BadRequest(errorMessage);
            }
    
        }

        [HttpPut]
        [HttpAuthorizationFilter]
        [HttpResourceFilter]
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
        [HttpAuthorizationFilter]
        [HttpResourceFilter]
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
