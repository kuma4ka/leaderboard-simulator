using System.Xml.Serialization;
using Leaderboard.Shared;
using Leaderboard.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace LeaderboardSimulator.DataAccess;

public class XmlGameRepository(ILogger<XmlGameRepository> logger) : IGameRepository
{
    private const string FileName = "game.xml";

    public async Task SaveAsync(GameDTO? gameDTO)
    {
        ArgumentNullException.ThrowIfNull(gameDTO);
        var xmlSerializer = new XmlSerializer(typeof(GameDTO));

        try
        {
            await using var fs = new FileStream(FileName, FileMode.Create);
            xmlSerializer.Serialize(fs, gameDTO);
            logger.LogInformation("Game state saved successfully to XML.");
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
            if (!File.Exists(FileName))
            {
                logger.LogWarning("Game file not found, initializing a new game.");
                return new GameDTO();
            }

            await using var fs = new FileStream(FileName, FileMode.Open);
            var gameDTO = xmlSerializer.Deserialize(fs) as GameDTO;
            
            if (gameDTO == null) throw new InvalidOperationException("Failed to deserialize game.");

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