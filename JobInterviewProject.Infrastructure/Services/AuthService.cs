using JobInterviewProject.Domain.Exceptions;
using JobInterviewProject.Domain.Models;
using JobInterviewProject.Infrastructure.Database.UnitOfWork;
using JobInterviewProject.Infrastructure.DTOs;
using JobInterviewProject.Infrastructure.Services.Service_Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobInterviewProject.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthService(IUnitOfWork unitOfWork, UserManager<User> userManager, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthResultDto> Login(LoginDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var user = await _unitOfWork.AuthRepository.Login(model.Email);
            var userCheck = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user != null && userCheck is true)
            {
                var tokenValue = await GenerateJWTTokenAsync(user, null);

                return tokenValue;
            }

            return null;
        }

        public async Task<User> Register(RegisterDto model)
        {
            var user = new User();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null && await _userManager.FindByIdAsync(model.PersonalId) != null)
            {
                throw new PersonalIdAlreadyExistsException($"Personal Id: {model.PersonalId} already exists");
            }

            var passwordHash = new PasswordHasher<User>();
            var gender = (Domain.Models.Enums.Gender)model.UserGender;
            user.PersonalId = model.PersonalId;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.UserGender = gender.ToString();
            user.PasswordHash = passwordHash.HashPassword(user, model.Password);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.DateOfBirth = model.DateOfBirth;

            return await _unitOfWork.AuthRepository.Register(user);
        }

        public async Task<AuthResultDto> RequestNewToken(RefreshTokenDto refreshToken)
        {
            var rToken = await VerifyAndGenerateTokenAsync(refreshToken);

            return rToken;
        }

        public async Task<AuthResultDto> VerifyAndGenerateTokenAsync(RefreshTokenDto refreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _unitOfWork.AuthRepository.GetRefreshTokenAsync(refreshToken.RefreshToken);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == storedToken.UserId);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(refreshToken.Token, _tokenValidationParameters, out var validToken);

                return await GenerateJWTTokenAsync(user, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateJWTTokenAsync(user, storedToken);
                }
                else
                {
                    return await GenerateJWTTokenAsync(user, null);
                }
            }
        }

        public async Task<AuthResultDto> GenerateJWTTokenAsync(User user, RefreshTokenModel rToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if (rToken != null)
            {
                var rTokenResponse = new AuthResultDto()
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }

            var refreshToken = new RefreshTokenModel()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _unitOfWork.AuthRepository.AddRefreshTokenAsync(refreshToken);

            var response = new AuthResultDto()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
            return response;
        }
    }
}
