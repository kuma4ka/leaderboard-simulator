namespace Leaderboard.Shared.DTOs;

[Serializable]
public class PlayerDTO
{
    public string? Name { get; set; }
    public int Score { get; set; }
}