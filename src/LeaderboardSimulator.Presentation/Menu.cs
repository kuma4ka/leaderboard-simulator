using LeaderboardSimulator.Logic;
using LeaderboardSimulator.Logic.Interfaces.Services;
using LeaderboardSimulator.Logic.Models;
using Microsoft.Extensions.Logging;

namespace LeaderboardSimulator.Presentation;

public class Menu(
    ConcurrentMatchSimulator simulator,
    Game game,
    IGameService gameService,
    ILogger<Menu> logger)
{
    public async Task DisplayAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Leaderboard Simulator ===");
            Console.WriteLine($"Current Game: {game.Matches.Count} matches active");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("1. Simulate Matches (Multithreaded)");
            Console.WriteLine("2. View Leaderboards");
            Console.WriteLine("3. Save & Exit");
            Console.Write("\nSelect an option: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SimulateMatches();
                    break;
                case "2":
                    ViewLeaderboards();
                    break;
                case "3":
                    await SaveGameStateAsync();
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void SimulateMatches()
    {
        Console.WriteLine("\nSimulating matches...");
        
        var degreeOfParallelism = Environment.ProcessorCount;
        
        simulator.Simulate(degreeOfParallelism);
        
        Console.WriteLine("Matches have been simulated!");
        logger.LogInformation("Matches simulated using {Degree} threads.", degreeOfParallelism);
        
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private void ViewLeaderboards()
    {
        Console.Clear();
        foreach (var match in game.Matches)
        {
            Console.WriteLine($"\n--- Match: {match.MatchId} ---");
            Console.WriteLine("{0,-20} | {1,5}", "Player", "Score");
            Console.WriteLine(new string('-', 30));

            foreach (var player in match.Leaderboard.Players)
            {
                Console.WriteLine($"{player.Name,-20} | {player.Score,5}");
            }
        }
        
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task SaveGameStateAsync()
    {
        Console.WriteLine("Saving game state...");
        await gameService.SaveGameAsync(game);
        logger.LogInformation("Game saved manually by user.");
        Console.WriteLine("Saved. Goodbye!");
        Thread.Sleep(1000); 
    }
}