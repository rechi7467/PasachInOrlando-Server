using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IUserService
    {
        public List<UserAdminDto> GetAllActiveUsers();
        public List<UserAdminDto> GetAllUsersIncludingInactive();
        public UserResponseDto GetUserById(int id);
        public UserAdminDto GetUserByIdForAdmin(int id);
        public void CreateUser(CreateUserDto dto);
        public void UpdateUser(int id, UpdateUserDto dto);
        public void DeleteUser(int id);
        public AuthResponseDto Login(LoginDto dto);
        public void SetPassword(SetPasswordDto dto);
        public AuthResponseDto RegisterUser(RegisterUserDto dto);
    }
}
