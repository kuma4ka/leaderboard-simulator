namespace Leaderboard.Shared.DTOs;

[Serializable]
public class LeaderboardDTO
{
    public Guid LeaderboardId { get; set; }
    public List<PlayerDTO> Players { get; set; } = new();
}