using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Implementations;

[Serializable]
public class Leaderboard
{
    public Guid LeaderboardId { get; set; } = Guid.NewGuid();
    public List<Player> Players { get; set; } = new();
    private readonly ILeaderboardSorter? _leaderboardSorter;

    public Leaderboard() { }

    public Leaderboard(ILeaderboardSorter sorter)
    {
        _leaderboardSorter = sorter;
        Players = new List<Player>();
    }

    public void UpdateLeaderboard()
    {
        if (_leaderboardSorter is null) return;
        Players = _leaderboardSorter.Sort(Players).ToList();
    }
}