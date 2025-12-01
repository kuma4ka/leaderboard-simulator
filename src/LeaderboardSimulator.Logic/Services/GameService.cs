using Leaderboard.Shared;
using LeaderboardSimulator.Logic.Interfaces.Mappers;
using LeaderboardSimulator.Logic.Interfaces.Services;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Services;

public class GameService(IGameCache gameCache, IGameMapper gameMapper) : IGameService
{
    public async Task<Game?> GetGameAsync()
    {
        var gameDTO = await gameCache.LoadAsync();
        return gameDTO is null ? null : gameMapper.ToModel(gameDTO);
    }

    public async Task SaveGameAsync(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);
        var gameDTO = gameMapper.ToDTO(game);
        await gameCache.SaveAsync(gameDTO);
    }
}