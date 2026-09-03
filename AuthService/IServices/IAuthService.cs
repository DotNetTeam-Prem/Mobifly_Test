using AuthService.DTOs;

namespace AuthService.IServices
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);

        Task<AuthResponse> LoginAsync(LoginRequest request);

        Task<AuthResponse> GetMeAsync(Guid userId);
    }
}