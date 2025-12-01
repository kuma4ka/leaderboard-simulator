namespace Leaderboard.Shared.DTOs;

[Serializable]
public class GameDTO
{
    public List<MatchDTO> Matches { get; set; } = new();
}