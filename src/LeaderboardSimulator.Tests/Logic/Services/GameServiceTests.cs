using Leaderboard.Shared;
using Leaderboard.Shared.DTOs;
using LeaderboardSimulator.Logic.Interfaces.Mappers;
using LeaderboardSimulator.Logic.Services;
using Moq;

namespace LeaderboardSimulator.Tests.Logic.Services;

public class GameServiceTests
{
    private readonly Mock<IGameCache> _mockCache;
    private readonly Mock<IGameMapper> _mockMapper;
    private readonly GameService _service;

    public GameServiceTests()
    {
        _mockCache = new Mock<IGameCache>();
        _mockMapper = new Mock<IGameMapper>();
        _service = new GameService(_mockCache.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetGameAsync_ShouldReturnNull_WhenCacheReturnsNull()
    {
        // Arrange
        _mockCache.Setup(c => c.LoadAsync()).ReturnsAsync((GameDTO?)null);

        // Act
        var result = await _service.GetGameAsync();

        // Assert
        Assert.Null(result);
        
        _mockMapper.Verify(m => m.ToModel(It.IsAny<GameDTO>()), Times.Never);
    }
}