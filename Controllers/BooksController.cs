using BookFinderApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest("Search query is required");

            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            var books = await _bookService.SearchBooks(search, userId);
            return Ok(books);
        }


    }
}