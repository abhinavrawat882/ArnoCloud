using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArnoCloud.User.Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Service.Helpers;

internal class JwtHelper
{   
    private readonly JwtOption _jwtOption;
    public JwtHelper(JwtOption jwtOption)
    {
        _jwtOption=jwtOption;        
    }
    public  string GenerateJwtToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOption.Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public List<Claim> BuildClaims(User user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email)
    };

    foreach (var role in user.UserRoles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
    }

    return claims;
}
}
