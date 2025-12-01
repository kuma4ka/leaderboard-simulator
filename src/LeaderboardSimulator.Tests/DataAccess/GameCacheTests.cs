using Leaderboard.Shared;
using Leaderboard.Shared.DTOs;
using LeaderboardSimulator.DataAccess;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace LeaderboardSimulator.Tests.DataAccess;

public class GameCacheTests
{
    private readonly Mock<IGameRepository> _mockRepo;
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<ILogger<GameCache>> _mockLogger;
    private readonly GameCache _gameCache;

    public GameCacheTests()
    {
        _mockRepo = new Mock<IGameRepository>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _mockLogger = new Mock<ILogger<GameCache>>();
        _gameCache = new GameCache(_memoryCache, _mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task LoadAsync_ShouldReturnFromCache_IfKeyExists()
    {
        // Arrange
        var cachedGame = new GameDTO();
        _memoryCache.Set("Game", cachedGame);

        // Act
        var result = await _gameCache.LoadAsync();

        // Assert
        Assert.Same(cachedGame, result);
        _mockRepo.Verify(r => r.LoadAsync(), Times.Never);
    }

    [Fact]
    public async Task LoadAsync_ShouldLoadFromRepo_IfCacheMiss()
    {
        // Arrange
        var repoGame = new GameDTO();
        _mockRepo.Setup(r => r.LoadAsync()).ReturnsAsync(repoGame);

        // Act
        var result = await _gameCache.LoadAsync();

        // Assert
        Assert.Same(repoGame, result);
        Assert.True(_memoryCache.TryGetValue("Game", out _));
        _mockRepo.Verify(r => r.LoadAsync(), Times.Once);
    }
}