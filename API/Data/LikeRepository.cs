using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLikeAsync(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<AppUser> GetUserWithLikesAsync(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikesAsync(string predicate, int userId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            switch (predicate)
            {
                case "liked":
                    likes = likes.Where(like => like.SourceUserId == userId);
                    users = likes.Select(like => like.TargetUser);
                    break;
                case "likedBy":
                    likes = likes.Where(like => like.TargetUserId == userId);
                    users = likes.Select(like => like.SourceUser);
                    break;
            }

            return await users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            }).ToListAsync();
        }
    }
}
