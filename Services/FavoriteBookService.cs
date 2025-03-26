using BookFinderApi.DTOs;
using BookFinderApi.Interfaces;
using BookFinderApi.Models;

namespace BookFinderApi.Services
{
    public class FavoriteBookService
    {
        private readonly IFavoriteBookRepository _favoriteBookRepository;

        public FavoriteBookService(IFavoriteBookRepository favoriteBookRepository)
        {
            _favoriteBookRepository = favoriteBookRepository;
        }

        public async Task<IEnumerable<FavoriteBook>> GetUserFavorites(int userId)
        {
            return await _favoriteBookRepository.GetByUserIdAsync(userId);
        }

        public async Task<FavoriteBook> GetFavoriteBook(int userId, string bookId)
        {
            return await _favoriteBookRepository.GetByUserAndBookIdAsync(userId, bookId);
        }

        public async Task<IEnumerable<FavoriteBookDto>> SearchFavorites(int userId, string query)
        {
            var normalizedQuery = query.Trim().ToLower();
            var favorites = await _favoriteBookRepository.GetByUserIdAsync(userId);

            return favorites
                .Where(f => f.Title.ToLower().Contains(normalizedQuery) ||
                           f.Author.ToLower().Contains(normalizedQuery))
                .Select(f => new FavoriteBookDto
                {
                    Id = f.Id,
                    BookId = f.BookId,
                    Title = f.Title,
                    Author = f.Author,
                    CoverImageUrl = f.CoverImageUrl,
                    AddedDate = f.AddedDate
                });
        }

        public async Task<FavoriteBook> AddFavorite(int userId, AddFavoriteDto dto)
        {
            var favorite = new FavoriteBook
            {
                UserId = userId,
                BookId = dto.BookId,
                Title = dto.Title,
                Author = dto.Author,
                CoverImageUrl = dto.CoverImageUrl
            };
            await _favoriteBookRepository.AddAsync(favorite);
            return favorite;
        }

        public async Task RemoveFavorite(int userId, string bookId)
        {
            var favorite = await _favoriteBookRepository.GetByUserAndBookIdAsync(userId, bookId);
            if (favorite != null)
            {
                await _favoriteBookRepository.DeleteAsync(favorite);
            }
        }
    }
}
