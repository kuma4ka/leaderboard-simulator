namespace LeaderboardSimulator.Logic.Models;

[Serializable]
public class Game
{
    public List<Match> Matches { get; set; } = new();
    
    public Game() { }
    
    public Game(List<Match> matches) 
    {
        Matches = matches ?? throw new ArgumentNullException(nameof(matches));
    }
}