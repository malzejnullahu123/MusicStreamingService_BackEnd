using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;


namespace MusicStreamingService_BackEnd.Services.FollowService;

public class FollowService : IFollowService
{
    private readonly AppDbContext _dbContext;
    private readonly ExtractFromToken _extractor;


    public FollowService(AppDbContext dbContext, ExtractFromToken extractor)
    {
        _dbContext = dbContext;
        _extractor = extractor;
    }
    
    
    public async Task<bool> FollowUser(string token, int followingUserId)
    {
        var userId = _extractor.Id(token);
        
        // Check if the follow relationship already exists
        var existingFollow = await _dbContext.Follows
            .Where(f => f.UserID == userId && f.FollowingUserID == followingUserId)
            .FirstOrDefaultAsync();
        
        if (existingFollow != null)
        {
            return false; // Already following
        }
        
        // Create a new follow relationship
        var follow = new Follow
        {
            UserID = userId,
            FollowingUserID = followingUserId
        };
        
        _dbContext.Follows.Add(follow);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    
    
    public async Task<bool> UnfollowUser(string token, int followingUserId)
    {
        var userId = _extractor.Id(token);
        
        var follow = await _dbContext.Follows
            .Where(f => f.UserID == userId && f.FollowingUserID == followingUserId)
            .FirstOrDefaultAsync();
        
        if (follow == null)
        {
            return false; // Not following
        }
        
        _dbContext.Follows.Remove(follow);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> IsFollowing(string token, int followingUserId)
    {
        var userId = _extractor.Id(token);
        
        var follow = await _dbContext.Follows
            .Where(f => f.UserID == userId && f.FollowingUserID == followingUserId)
            .FirstOrDefaultAsync();
        
        return follow != null;
    }

    public async Task<FollowCountModel> GetFollowerCounts(string token)
    {
        var userId = _extractor.Id(token);
        var followerCount = await _dbContext.Follows
            .Where(u => u.FollowingUserID == userId)
            .CountAsync();
    
        var followingCount = await _dbContext.Follows
            .Where(u => u.UserID == userId)
            .CountAsync();

        return new FollowCountModel
        {
            FollowerCount = followerCount,
            FollowingCount = followingCount
        };
    }
    
}