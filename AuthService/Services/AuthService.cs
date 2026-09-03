using AuthService.DTOs;
using AuthService.Entities;
using AuthService.IRepositories;
using AuthService.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingUser =
                await _userRepository.GetByUsernameAsync(request.Username);

            if (existingUser != null)
                throw new Exception("Username already exists.");

            if (request.Role != "ADMIN" && request.Role != "USER")
                throw new Exception("Invalid role.");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);
            _logger.LogInformation(
    "User registered successfully: {Username}",
    user.Username);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user =
                await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new Exception("Invalid username or password.");

            if (!user.IsActive)
                throw new Exception("User is inactive.");

            bool validPassword =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!validPassword)
                throw new Exception("Invalid username or password.");

            _logger.LogInformation(
    "User logged in successfully: {Username}",
    user.Username);
            var claims = new[]
            {
    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Role, user.Role)
};

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<AuthResponse> GetMeAsync(Guid userId)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found.");

            return new AuthResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}