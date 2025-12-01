using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Mappers;
using LeaderboardSimulator.Logic.Models;
using Match = LeaderboardSimulator.Logic.Models.Match;
using Moq;

namespace LeaderboardSimulator.Tests.Logic.Mappers;

public class GameMapperTests
{
    [Fact]
    public void ToDTO_ShouldMapGameToGameDTO_Correctly()
    {
        // Arrange
        var mockSorter = new Mock<ILeaderboardSorter>();
        var mapper = new GameMapper(mockSorter.Object);

        var game = new Game
        {
            Matches = new List<Match>
            {
                new Match
                {
                    MatchId = Guid.NewGuid(),
                    Players = new List<Player> { new() { Name = "TestPlayer", Score = 100 } }
                }
            }
        };

        // Act
        var result = mapper.ToDTO(game);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Matches);
        Assert.Equal(game.Matches[0].MatchId, result.Matches[0].MatchId);
        Assert.Equal("TestPlayer", result.Matches[0].Players[0].Name);
    }
}