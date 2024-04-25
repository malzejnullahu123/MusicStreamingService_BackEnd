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
        
        var idClaim = principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (idClaim == null || !int.TryParse(idClaim.Value, out var id))
        {
            throw new ArgumentException("UnAuthorized");
        }
        
        var user = _dbContext.Users.FirstOrDefault(user => user.UserId == id);

        if (user == null)
        {
            throw new ArgumentException("UnAuthorized3 su gjet");
        }

        return user.UserId;
    }
    
}