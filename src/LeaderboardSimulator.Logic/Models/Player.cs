namespace LeaderboardSimulator.Logic.Models;

[Serializable]
public class Player
{
    public string? Name { get; set; }
    public int Score { get; set; }

    public void AddScore(int points) => Score += points;
}