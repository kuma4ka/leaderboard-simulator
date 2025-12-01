namespace Leaderboard.Shared.DTOs;

[Serializable]
public class MatchDTO
{
    public Guid MatchId { get; set; }
    public List<PlayerDTO> Players { get; set; } = new();
    public LeaderboardDTO Leaderboard { get; set; } = new();
}