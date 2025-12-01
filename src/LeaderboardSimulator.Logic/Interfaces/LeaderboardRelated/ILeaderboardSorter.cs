using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;

public interface ILeaderboardSorter
{
    IEnumerable<Player> Sort(IEnumerable<Player> players);
}