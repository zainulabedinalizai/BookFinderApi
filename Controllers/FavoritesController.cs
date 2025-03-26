using BookFinderApi.DTOs;
using BookFinderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookFinderApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoriteBookService _favoriteBookService;
        private readonly BookService _bookService;
        private readonly int _userId;

        public FavoritesController(
            FavoriteBookService favoriteBookService,
            BookService bookService,
            IHttpContextAccessor httpContextAccessor)
        {
            _favoriteBookService = favoriteBookService;
            _bookService = bookService;
            _userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavorites()
        {
            var favorites = await _favoriteBookService.GetUserFavorites(_userId);
            return Ok(favorites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoriteBook(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Book ID is required");

            try
            {
                var favorite = await _favoriteBookService.GetFavoriteBook(_userId, id);
                if (favorite == null)
                    return NotFound();

                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching favorite book: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFavorites([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is required");

            try
            {
                var favorites = await _favoriteBookService.SearchFavorites(_userId, query);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error searching favorites: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteDto addFavoriteDto)
        {
            var favorite = await _favoriteBookService.AddFavorite(_userId, addFavoriteDto);
            return CreatedAtAction(nameof(GetUserFavorites), favorite);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFavorite(string bookId)
        {
            await _favoriteBookService.RemoveFavorite(_userId, bookId);
            return NoContent();
        }
    }
}