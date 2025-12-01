using LeaderboardSimulator.Logic.Implementations;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Tests.Logic.Implementations;

public class ScoreBasedSorterTests
{
    [Fact]
    public void Sort_ShouldReturnPlayers_OrderedByScoreDescending()
    {
        // Arrange
        var sorter = new ScoreBasedSorter();
        var players = new List<Player>
        {
            new() { Name = "Noob", Score = 10 },
            new() { Name = "Pro", Score = 100 },
            new() { Name = "Mid", Score = 50 }
        };

        // Act
        var result = sorter.Sort(players).ToList();

        // Assert
        Assert.Equal("Pro", result[0].Name);
        Assert.Equal("Mid", result[1].Name);
        Assert.Equal("Noob", result[2].Name);
        Assert.Equal(100, result[0].Score);
    }

    [Fact]
    public void Sort_ShouldHandleEmptyList()
    {
        // Arrange
        var sorter = new ScoreBasedSorter();
        var players = new List<Player>();

        // Act
        var result = sorter.Sort(players);

        // Assert
        Assert.Empty(result);
    }
}