using Leaderboard.Shared.DTOs;
using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Interfaces.Mappers;
using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Mappers;

public class GameMapper(ILeaderboardSorter leaderboardSorter) : IGameMapper
{
    public GameDTO ToDTO(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);

        return new GameDTO
        {
            Matches = game.Matches.Select(m => new MatchDTO
            {
                MatchId = m.MatchId,
                Players = m.Players.Select(p => new PlayerDTO
                {
                    Name = p.Name,
                    Score = p.Score
                }).ToList(),
                
                Leaderboard = new LeaderboardDTO
                {
                    LeaderboardId = m.Leaderboard.LeaderboardId,
                    Players = m.Leaderboard.Players.Select(lp => new PlayerDTO
                    {
                        Name = lp.Name,
                        Score = lp.Score
                    }).ToList()
                }
            }).ToList()
        };
    }

    public Game ToModel(GameDTO gameDTO)
    {
        ArgumentNullException.ThrowIfNull(gameDTO);

        return new Game
        {
            Matches = gameDTO.Matches.Select(m => new Match
            {
                MatchId = m.MatchId,
                Players = m.Players.Select(p => new Player
                {
                    Name = p.Name,
                    Score = p.Score
                }).ToList(),

                Leaderboard = new Implementations.Leaderboard(leaderboardSorter)
                {
                    LeaderboardId = m.Leaderboard.LeaderboardId,
                    Players = m.Leaderboard.Players.Select(lp => new Player
                    {
                        Name = lp.Name,
                        Score = lp.Score
                    }).ToList()
                }
            }).ToList()
        };
    }
}