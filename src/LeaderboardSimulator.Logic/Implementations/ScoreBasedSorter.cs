using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Implementations;

public class ScoreBasedSorter : ILeaderboardSorter
{
    public IEnumerable<Player> Sort(IEnumerable<Player> players)
    {
        return players.OrderByDescending(p => p.Score);
    }
}