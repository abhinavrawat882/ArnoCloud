using User.Service.DTO;

namespace User.Service.Interfaces;

public interface IUserService
{
    Task<UserResponse> AddUserAsync(CreateUserRequest request);
    Task UpdatePasswordAsync(int userId, UpdatePasswordRequest request);
    Task DeleteUserAsync(int userId);
    Task SetRoleAsync(int userId, SetUserRoleRequest request);
}
