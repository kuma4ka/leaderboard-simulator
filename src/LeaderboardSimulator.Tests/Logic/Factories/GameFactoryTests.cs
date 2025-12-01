using LeaderboardSimulator.Logic.Factories;
using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Interfaces.PlayerManaging;
using LeaderboardSimulator.Logic.Models;
using Moq;

namespace LeaderboardSimulator.Tests.Logic.Factories;

public class GameFactoryTests
{
    [Fact]
    public void CreateGame_ShouldCreateCorrectNumberOfMatches()
    {
        // Arrange
        var mockSorter = new Mock<ILeaderboardSorter>();
        var mockManager = new Mock<IMatchPlayerManager>();

        mockManager
            .Setup(m => m.GetTopPlayers(It.IsAny<IEnumerable<Player>>(), It.IsAny<int>()))
            .Returns(new List<Player>());

        var factory = new GameFactory(mockSorter.Object, mockManager.Object);
        int matchCount = 5;
        int playersPerMatch = 10;

        // Act
        var game = factory.CreateGame(matchCount, playersPerMatch);

        // Assert
        Assert.NotNull(game);
        Assert.Equal(matchCount, game.Matches.Count);
        
        Assert.All(game.Matches, match => 
            Assert.Equal(playersPerMatch, match.Players.Count));
    }
}