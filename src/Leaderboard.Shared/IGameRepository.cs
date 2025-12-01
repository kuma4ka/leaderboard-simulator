using Leaderboard.Shared.DTOs;

namespace Leaderboard.Shared;

public interface IGameRepository
{
    ValueTask<GameDTO?> LoadAsync();
    Task SaveAsync(GameDTO? gameDTO);
}