using System.Xml.Serialization;
using Leaderboard.Shared;
using Leaderboard.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace LeaderboardSimulator.DataAccess;

public class XmlGameRepository(ILogger<XmlGameRepository> logger) : IGameRepository
{
    private readonly string _filePath = Path.Combine("Data", "game.xml");

    public async Task SaveAsync(GameDTO? gameDTO)
    {
        ArgumentNullException.ThrowIfNull(gameDTO);
        var xmlSerializer = new XmlSerializer(typeof(GameDTO));

        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var fs = new FileStream(_filePath, FileMode.Create);
            xmlSerializer.Serialize(fs, gameDTO);
            logger.LogInformation("Game state saved successfully to XML at {Path}.", _filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save game state to XML.");
            throw;
        }
    }

    public async ValueTask<GameDTO?> LoadAsync()
    {
        var xmlSerializer = new XmlSerializer(typeof(GameDTO));
        
        try
        {
            if (!File.Exists(_filePath))
            {
                logger.LogWarning("Game file not found at {Path}, initializing a new game.", _filePath);
                return new GameDTO();
            }

            await using var fs = new FileStream(_filePath, FileMode.Open);
            var gameDTO = xmlSerializer.Deserialize(fs) as GameDTO;
            
            if (gameDTO is null) throw new InvalidOperationException("Failed to deserialize game.");

            logger.LogInformation("Loaded game with {Count} matches.", gameDTO.Matches.Count);
            return gameDTO;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while loading the game.");
            throw;
        }
    }
}