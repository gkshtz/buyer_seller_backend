using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BuyerSeller.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace TicketBookingHub.Helpers
{
    /// <summary>
    /// JwtTokenService
    /// </summary>
    public static class JwtTokenService
    {
        /// <summary>
        /// Generates the JWT token
        /// </summary>
        /// <param name="user">Details of the user</param>
        /// <param name="roles">Roles assigned to the user</param>
        /// <param name="_configuration">IConfiguration</param> 
        /// <returns>Token</returns>
        public static string GenerateToken(User user, string role, IConfiguration _configuration)
        {
            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim("UserId",user.UserId.ToString()),
                        new Claim("FirstName",user.FirstName),
                        new Claim("LastName",user.LastName),
                        new Claim("Email",user.Email),
                        new Claim(ClaimTypes.Role,role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
