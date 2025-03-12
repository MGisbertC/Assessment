using MGisbert.Appointments.Data.Repositories;
using MGisbert.Appointments.Models.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MGisbert.Appointments.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetSingleASync(p => p.Email == loginRequest.Email,  o => o.Role);

            //I couldn't finish the implementation of the password hashing, so I'm using a hardcoded password for the demo
            if (loginRequest.Email == user?.Email && loginRequest.Password == "password")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                //this MUST NOT be here, it should be in a configuration file, but is for demo purposes
                var key = Encoding.UTF8.GetBytes("your-very-secure-and-long-secret-key");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Email, loginRequest.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "MGisbert.Appoinments", 
                    Audience = "Testaudience", 
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            else
            {
                throw new ArgumentException("Invalid credentials");
            }
        }
    }
}
