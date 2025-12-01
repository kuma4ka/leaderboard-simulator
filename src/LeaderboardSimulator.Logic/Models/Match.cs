using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Interfaces.PlayerManaging;

namespace LeaderboardSimulator.Logic.Models;

[Serializable]
public class Match
{
    public Guid MatchId { get; set; } = Guid.NewGuid();
    public List<Player> Players { get; set; } = new();
    public Implementations.Leaderboard Leaderboard { get; set; } = new();

    public Match() { }

    public Match(IMatchPlayerManager matchPlayerManager, ILeaderboardSorter sorter, IEnumerable<Player> players, int leaderboardTopCount = 5)
    {
        Players = new List<Player>(players);
        Leaderboard = new Implementations.Leaderboard(sorter);
        
        foreach (var player in matchPlayerManager.GetTopPlayers(Players, leaderboardTopCount))
        {
            Leaderboard.Players.Add(player);
        }
        Leaderboard.UpdateLeaderboard();
    }

    public void ScorePlayer(string playerName, int points)
    {
        var player = Players.SingleOrDefault(p => p.Name == playerName);
        if (player is null) return;

        player.AddScore(points);
        Leaderboard.UpdateLeaderboard();
    }
}