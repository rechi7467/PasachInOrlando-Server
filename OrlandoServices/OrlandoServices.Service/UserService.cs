using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrlandoServices.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDays = int.Parse(_configuration["Jwt:ExpiryDays"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
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

        // פונקציית עזר פרטית — ממירה User ל-UserAdminDto
        // כותבים פעם אחת ומשתמשים בכל פונקציות המנהל
        private UserAdminDto ToAdminDto(User user) => new UserAdminDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            BillingAddress = user.BillingAddress,
            City = user.City,
            State = user.State,
            ZipCode = user.ZipCode,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            TotalOrders = user.Orders.Count,
            TotalDonations = user.Donations.Count
        };

        //פונקציה פשוטה לבדוק אם האימייל תקין
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public void CreateUser(CreateUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // בדיקות חובה
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ArgumentException("FirstName is required");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ArgumentException("LastName is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Invalid email format");

            if (_userRepository.ExistsByEmail(dto.Email))
                throw new ArgumentException("Email already exists");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                throw new ArgumentException("Phone is required");

            if (string.IsNullOrWhiteSpace(dto.BillingAddress))
                throw new ArgumentException("BillingAddress is required");

            if (string.IsNullOrWhiteSpace(dto.City))
                throw new ArgumentException("City is required");

            if (string.IsNullOrWhiteSpace(dto.State))
                throw new ArgumentException("State is required");

            if (string.IsNullOrWhiteSpace(dto.ZipCode))
                throw new ArgumentException("ZipCode is required");

            // יצירת ישות
            var user = new User
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone.Trim(),
                BillingAddress = dto.BillingAddress.Trim(),
                City = dto.City.Trim(),
                State = dto.State.Trim(),
                ZipCode = dto.ZipCode.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.Add(user);
            _userRepository.SaveChanges();
        }
        public void UpdateUser(int id, UpdateUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
                throw new NotFoundException($"User with id {id} not found");

            // FirstName
            if (dto.FirstName != null)
            {
                if (string.IsNullOrWhiteSpace(dto.FirstName))
                    throw new ArgumentException("FirstName cannot be empty");

                existingUser.FirstName = dto.FirstName;
            }

            // LastName
            if (dto.LastName != null)
            {
                if (string.IsNullOrWhiteSpace(dto.LastName))
                    throw new ArgumentException("LastName cannot be empty");

                existingUser.LastName = dto.LastName;
            }

            // Email
            if (dto.Email != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException("Email cannot be empty");

                if (!IsValidEmail(dto.Email))
                    throw new ArgumentException("Invalid email format");

                if (dto.Email != existingUser.Email &&
                    _userRepository.ExistsByEmail(dto.Email))
                    throw new ArgumentException("Email already exists");

                existingUser.Email = dto.Email;
            }

            // Phone
            if (dto.Phone != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Phone))
                    throw new ArgumentException("Phone cannot be empty");

                existingUser.Phone = dto.Phone;
            }

            // BillingAddress
            if (dto.BillingAddress != null)
            {
                if (string.IsNullOrWhiteSpace(dto.BillingAddress))
                    throw new ArgumentException("BillingAddress cannot be empty");

                existingUser.BillingAddress = dto.BillingAddress;
            }

            // City
            if (dto.City != null)
            {
                if (string.IsNullOrWhiteSpace(dto.City))
                    throw new ArgumentException("City cannot be empty");

                existingUser.City = dto.City;
            }

            // State
            if (dto.State != null)
            {
                if (string.IsNullOrWhiteSpace(dto.State))
                    throw new ArgumentException("State cannot be empty");

                existingUser.State = dto.State;
            }

            // ZipCode
            if (dto.ZipCode != null)
            {
                if (string.IsNullOrWhiteSpace(dto.ZipCode))
                    throw new ArgumentException("ZipCode cannot be empty");

                existingUser.ZipCode = dto.ZipCode;
            }

            _userRepository.Update(existingUser);
            _userRepository.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            // שליפה
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} not found");

            // אם כבר לא פעיל – אין מה לעשות
            if (!user.IsActive)
                return;

            // Soft Delete
            user.IsActive = false;

            _userRepository.Update(user);
            _userRepository.SaveChanges();
        }

        public List<UserAdminDto> GetAllUsersIncludingInactive()
        {
            return _userRepository.GetAllWithDetails()
                .Select(u => ToAdminDto(u))
                .ToList();
        }
        public List<UserAdminDto> GetAllActiveUsers()
        {
            return _userRepository.GetAllWithDetails()
                .Where(u => u.IsActive)
                .Select(u => ToAdminDto(u))
                .ToList();
        }

        public UserAdminDto GetUserByIdForAdmin(int id)
        {
            var user = _userRepository.GetByIdWithDetails(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} not found");

            return ToAdminDto(user);
        }

        public UserResponseDto GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} not found");

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                BillingAddress = user.BillingAddress,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
        }

        public AuthResponseDto Login(LoginDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var user = _userRepository.GetByEmail(dto.Email.Trim());
            if (user == null || !user.IsActive)
                throw new ArgumentException("Invalid email or password");

            if (string.IsNullOrEmpty(user.PasswordHash))
                throw new ArgumentException("To log in, please set a password first.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ArgumentException("Invalid email or password");

            var expiryDays = int.Parse(_configuration["Jwt:ExpiryDays"]!);

            return new AuthResponseDto
            {
                Token = GenerateToken(user),
                UserId = user.Id,
                FirstName = user.FirstName,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddDays(expiryDays)
            };
        }

        public void SetPassword(SetPasswordDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var user = _userRepository.GetByEmail(dto.Email.Trim());
            if (user == null || !user.IsActive)
                throw new NotFoundException("User not found");

            if (!string.IsNullOrEmpty(user.PasswordHash))
                throw new InvalidOperationException("This account already has a password");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            _userRepository.Update(user);
            _userRepository.SaveChanges();
        }
    }
}
