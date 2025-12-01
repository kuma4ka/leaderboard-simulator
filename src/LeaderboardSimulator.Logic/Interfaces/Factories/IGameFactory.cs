using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic.Interfaces.Factories;

public interface IGameFactory
{
    Game CreateGame(int numberOfMatches, int playersPerMatch);
}