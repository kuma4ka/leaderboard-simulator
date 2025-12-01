using LeaderboardSimulator.Logic.Interfaces.Factories;
using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Interfaces.PlayerManaging;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Factories;

public class GameFactory(ILeaderboardSorter sorter, IMatchPlayerManager matchPlayerManager) : IGameFactory
{
    public Game CreateGame(int numberOfMatches, int playersPerMatch)
    {
        var matches = new List<Match>();

        for (var i = 0; i < numberOfMatches; i++)
        {
            var players = CreatePlayers($"Match {i + 1}", playersPerMatch);
            var match = new Match(matchPlayerManager, sorter, players);
            matches.Add(match);
        }

        return new Game(matches);
    }

    private static List<Player> CreatePlayers(string matchName, int count)
    {
        var players = new List<Player>();
        for (var i = 1; i <= count; i++)
        {
            players.Add(new Player { Name = $"{matchName} Player{i}", Score = 0 });
        }
        return players;
    }
}