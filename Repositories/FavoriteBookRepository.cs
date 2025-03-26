using BookFinderApi.Data;
using BookFinderApi.Interfaces;
using BookFinderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookFinderApi.Repositories
{
    public class FavoriteBookRepository : IFavoriteBookRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteBookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoriteBook>> GetAllAsync()
        {
            return await _context.FavoriteBooks.ToListAsync();
        }

        public async Task<FavoriteBook> GetByIdAsync(int id)
        {
            return await _context.FavoriteBooks.FindAsync(id);
        }

        public async Task AddAsync(FavoriteBook entity)
        {
            await _context.FavoriteBooks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FavoriteBook entity)
        {
            _context.FavoriteBooks.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FavoriteBook entity)
        {
            _context.FavoriteBooks.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FavoriteBook>> GetByUserIdAsync(int userId)
        {
            return await _context.FavoriteBooks
                .Where(fb => fb.UserId == userId)
                .OrderByDescending(fb => fb.AddedDate)
                .ToListAsync();
        }

        public async Task<FavoriteBook> GetByUserAndBookIdAsync(int userId, string bookId)
        {
            return await _context.FavoriteBooks
                .FirstOrDefaultAsync(fb => fb.UserId == userId && fb.BookId == bookId);
        }

        public async Task<bool> IsBookFavorite(int userId, string bookId)
        {
            return await _context.FavoriteBooks
                .AnyAsync(fb => fb.UserId == userId && fb.BookId == bookId);
        }
    }
}
