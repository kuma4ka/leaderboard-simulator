using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Interfaces.Services;

public interface IGameService
{
    Task<Game?> GetGameAsync();
    Task SaveGameAsync(Game game);
}