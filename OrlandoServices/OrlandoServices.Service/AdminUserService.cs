using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrlandoServices.Service
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IConfiguration _configuration;

        public AdminUserService(IAdminUserRepository adminUserRepository, IConfiguration configuration)
        {
            _adminUserRepository = adminUserRepository;
            _configuration = configuration;
        }

        private string GenerateToken(AdminUser admin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDays = int.Parse(_configuration["Jwt:ExpiryDays"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expiryDays),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void CreateAdmin(CreateAdminDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username is required");

            var existing = _adminUserRepository.GetByName(dto.Username.Trim());
            if (existing != null)
                throw new ArgumentException("Username already exists");

            var admin = new AdminUser
            {
                Username = dto.Username.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _adminUserRepository.Add(admin);
            _adminUserRepository.SaveChanges();
        }

        public AuthResponseDto Login(AdminLoginDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var admin = _adminUserRepository.GetByName(dto.Username.Trim());
            if (admin == null || !admin.IsActive)
                throw new ArgumentException("Invalid username or password");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash))
                throw new ArgumentException("Invalid username or password");

            var expiryDays = int.Parse(_configuration["Jwt:ExpiryDays"]!);

            return new AuthResponseDto
            {
                Token = GenerateToken(admin),
                UserId = admin.Id,
                FirstName = admin.Username,
                Email = string.Empty,
                ExpiresAt = DateTime.UtcNow.AddDays(expiryDays)
            };
        }
    }
}
