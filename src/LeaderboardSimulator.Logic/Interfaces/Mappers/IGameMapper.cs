using Leaderboard.Shared.DTOs;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Interfaces.Mappers;

public interface IGameMapper
{
    GameDTO ToDTO(Game game);
    Game ToModel(GameDTO gameDTO);
}