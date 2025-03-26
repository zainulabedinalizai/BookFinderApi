using BookFinderApi.Models;

namespace BookFinderApi.Interfaces
{
    public interface IFavoriteBookRepository : IRepository<FavoriteBook>
    {
        Task<IEnumerable<FavoriteBook>> GetByUserIdAsync(int userId);
        Task<FavoriteBook> GetByUserAndBookIdAsync(int userId, string bookId);
        Task<bool> IsBookFavorite(int userId, string bookId);
    }
}
