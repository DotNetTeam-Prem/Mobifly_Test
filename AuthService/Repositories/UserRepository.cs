using AuthService.Entities;
using AuthService.IRepositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<User>(
                "sp_User_GetByUsername",
                new
                {
                    Username = username
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<User>(
                "sp_User_GetById",
                new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task CreateAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_User_Create",
                new
                {
                    user.UserId,
                    user.Username,
                    user.PasswordHash,
                    user.Role,
                    user.IsActive,
                    user.CreatedAt
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}