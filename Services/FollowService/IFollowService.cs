using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.FollowService;

public interface IFollowService
{
    Task<bool> FollowUser(string token, int followingUserId);
    Task<bool> UnfollowUser(string token, int followingUserId);
    Task<bool> IsFollowing(string token, int followingUserId);
    Task<FollowCountModel> GetFollowerCounts(string token);
}