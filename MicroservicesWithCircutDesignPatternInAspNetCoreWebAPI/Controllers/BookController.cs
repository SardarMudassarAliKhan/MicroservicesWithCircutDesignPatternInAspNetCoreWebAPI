using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_BAL.IService;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(nameof(GetAllBooks))]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching books.");
                return StatusCode(500, "An error occurred while fetching books.");
            }
        }

        [HttpGet(nameof(GetBookById))]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if(book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching the book.");
                return StatusCode(500, "An error occurred while fetching the book.");
            }
        }

        [HttpPost(nameof(AddBook))]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            try
            {
                var addedBook = await _bookService.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBookById), new { id = addedBook.Id }, addedBook);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An error occurred while adding the book.");
                return StatusCode(500, "An error occurred while adding the book.");
            }
        }

        [HttpPut(nameof(UpdateBook))]
        public async Task<ActionResult<Book>> UpdateBook(int id, Book book)
        {
            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(id, book);
                if(updatedBook == null)
                {
                    return NotFound();
                }
                return Ok(updatedBook);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An error occurred while updating the book.");
                return StatusCode(500, "An error occurred while updating the book.");
            }
        }

        [HttpDelete(nameof(DeleteBook))]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var deleted = await _bookService.DeleteBookAsync(id);
                if(!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An error occurred while deleting the book.");
                return StatusCode(500, "An error occurred while deleting the book.");
            }
        }
    }
}
