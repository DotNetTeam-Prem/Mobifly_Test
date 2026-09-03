using AuthService.Entities;

namespace AuthService.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByIdAsync(Guid userId);

        Task CreateAsync(User user);
    }
}