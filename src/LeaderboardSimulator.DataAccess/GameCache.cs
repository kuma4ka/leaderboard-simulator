using Leaderboard.Shared;
using Leaderboard.Shared.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LeaderboardSimulator.DataAccess;

public class GameCache(IMemoryCache memoryCache, IGameRepository repository, ILogger<GameCache> logger) 
    : IGameCache
{
    private const string CacheKey = "Game";

    public async ValueTask<GameDTO?> LoadAsync()
    {
        if (memoryCache.TryGetValue(CacheKey, out GameDTO? gameDTO))
        {
            logger.LogDebug("Game retrieved from cache.");
            return gameDTO;
        }

        logger.LogInformation("Cache miss. Loading game from repository.");
        gameDTO = await repository.LoadAsync();

        if (gameDTO != null)
        {
            logger.LogInformation("Game loaded from repository. Caching it.");
            memoryCache.Set(CacheKey, gameDTO, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });
        }

        return gameDTO;
    }

    public async Task SaveAsync(GameDTO? gameDTO)
    {
        if (gameDTO is null)
        {
            logger.LogError("Attempted to save a null game object.");
            throw new ArgumentNullException(nameof(gameDTO));
        }

        logger.LogInformation("Saving game to repository.");
        await repository.SaveAsync(gameDTO);

        logger.LogInformation("Invalidating cache entry.");
        memoryCache.Remove(CacheKey);
    }
}