using System.Security.Claims;
using MusicStreamingService_BackEnd.Database;

namespace MusicStreamingService_BackEnd.Services;

public class ExtractFromToken
{
    private readonly AppDbContext _dbContext;

    public ExtractFromToken(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public int Id(string token)
    {
        var principal = TokenService.VerifyToken(token);

        if (principal == null)
        {
            throw new ArgumentException("Invalid token: Token verification failed.");
        }

        var idClaim = principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (idClaim == null || !int.TryParse(idClaim.Value, out var id))
        {
            throw new ArgumentException("Invalid token: Missing or invalid user id claim.");
        }

        var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);

        if (user == null)
        {
            throw new ArgumentException("Invalid token: User not found.");
        }

        return user.UserId;
    }

    
}