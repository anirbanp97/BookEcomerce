using Bookstore.API.Models;
using Bookstore.BLL.Interfaces;
using Bookstore.Common;
using Bookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetBooks([FromQuery] string? search = null)
        {
            var books = _bookService.GetBooks(search);
            return Ok(ApiResponse<IEnumerable<Book>>.Ok(books));
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
                return NotFound(ApiResponse<string>.Fail("Book not found"));

            return Ok(ApiResponse<Book>.Ok(book));
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] BookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            var newId = _bookService.AddBook(book);
            return Ok(ApiResponse<int>.Ok(newId, "Book added successfully"));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookDto dto)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
                return NotFound(ApiResponse<string>.Fail("Book not found"));

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.Price = dto.Price;
            book.StockQuantity = dto.StockQuantity;

            _bookService.UpdateBook(book);
            return Ok(ApiResponse<string>.Ok("Book updated successfully"));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            _bookService.DeleteBook(id);
            return Ok(ApiResponse<string>.Ok("Book deleted successfully"));
        }
    }
}
