using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Interfaces.PlayerManaging;

public interface IMatchPlayerManager
{
    List<Player> GetTopPlayers(IEnumerable<Player> players, int topCount);
}