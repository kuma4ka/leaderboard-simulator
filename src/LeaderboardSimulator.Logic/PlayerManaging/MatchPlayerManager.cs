using LeaderboardSimulator.Logic.Interfaces.PlayerManaging;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.PlayerManaging;

public class MatchPlayerManager : IMatchPlayerManager
{
    public List<Player> GetTopPlayers(IEnumerable<Player> players, int count)
    {
        return players.OrderByDescending(p => p.Score).Take(count).ToList();
    }
}