using BookFinderApi.Models;

namespace BookFinderApi.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> SearchBooksAsync(string query);
    }
}
