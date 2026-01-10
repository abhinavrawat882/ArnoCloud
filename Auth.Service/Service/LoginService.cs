using System.IdentityModel.Tokens.Jwt;
using Auth.Service.DTO;
using Auth.Service.Entites;
using Auth.Service.Helpers;
using Auth.Service.Repository;

namespace Auth.Service.Service;

public class LoginService
{
    private readonly UserRepo _userRepo;
    private readonly JwtOption _jwtOption;
    public LoginService(UserRepo userRepo, JwtOption jwtOption)
    {
        _userRepo = userRepo;
        _jwtOption = jwtOption;
    }
    public async Task<LoginResponseDTO> Login(LoginCredDTO loginCred)
    {
        if (loginCred == null) throw new UnauthorizedAccessException("Invalid Credentials");
        var userDetails = await _userRepo.GetUserDetails(loginCred.username);
        if (userDetails == null) throw new UnauthorizedAccessException("Invalid Credentials");
        if (userDetails.Password == loginCred.password)
        {
            
            JwtHelper jwtHelper = new JwtHelper(_jwtOption);
            var claims = jwtHelper.BuildClaims(userDetails);
            var token = jwtHelper.GenerateJwtToken(claims);

            return new LoginResponseDTO
            {
                UserId = userDetails.Id,
                Username = userDetails.UserName,
                Email = userDetails.Email,
                Roles = userDetails.UserRoles.Select(r => r.Role.Name).ToList(),
                Token = token
            };
        }
        throw new UnauthorizedAccessException("Invalid Credentials");

    }
    

}