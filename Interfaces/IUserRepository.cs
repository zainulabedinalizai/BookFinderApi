using BookFinderApi.Models;

namespace BookFinderApi.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UserExists(string username);
    }
}
