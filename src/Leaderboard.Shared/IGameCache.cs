using Leaderboard.Shared.DTOs;

namespace Leaderboard.Shared;

public interface IGameCache
{
    ValueTask<GameDTO?> LoadAsync();
    Task SaveAsync(GameDTO? gameDTO);
}