using OrlandoServices.Core.DTOs;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IAdminUserService
    {
        AuthResponseDto Login(AdminLoginDto dto);
        void CreateAdmin(CreateAdminDto dto);
    }
}
