using JobInterviewProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JobInterviewProject.Infrastructure.Database.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> Register(User registerUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == registerUser.Email || x.UserName == registerUser.UserName);

            if (user != null)
            {
                throw new ArgumentException("Email or username already exists");
            }

            await _context.Users.AddAsync(registerUser);
            await _context.SaveChangesAsync();

            return registerUser;
        }

        public async Task AddRefreshTokenAsync(RefreshTokenModel refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTokenModel> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
