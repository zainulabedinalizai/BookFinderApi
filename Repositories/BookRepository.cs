using BookFinderApi.Data;
using BookFinderApi.Interfaces;
using BookFinderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookFinderApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddAsync(Book entity)
        {
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book entity)
        {
            _context.Books.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book entity)
        {
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                .ToListAsync();
        }
    }
}
