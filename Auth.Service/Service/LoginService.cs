using Auth.Service.DTO;
using Auth.Service.Entites;
using Auth.Service.Repository;

namespace Auth.Service.Service;

public class LoginService
{
    private readonly UserRepo _userRepo;
    public LoginService(UserRepo userRepo)
    {
         _userRepo=userRepo;
    }
    public async Task Login(LoginCredDTO loginCred)
    {
        if(loginCred==null) throw new UnauthorizedAccessException("Invalid Credentials");
        var userDetails = await _userRepo.GetUserDetails(loginCred.username);
        if(userDetails==null) throw new UnauthorizedAccessException("Invalid Credentials");
        if (userDetails.Password == loginCred.password)
        {
            
        }
    }
    
}