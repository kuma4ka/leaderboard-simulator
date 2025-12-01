using Leaderboard.Shared;
using LeaderboardSimulator.DataAccess;
using LeaderboardSimulator.Logic;
using LeaderboardSimulator.Logic.Factories;
using LeaderboardSimulator.Logic.Implementations;
using LeaderboardSimulator.Logic.Interfaces.Factories;
using LeaderboardSimulator.Logic.Interfaces.LeaderboardRelated;
using LeaderboardSimulator.Logic.Interfaces.Mappers;
using LeaderboardSimulator.Logic.Interfaces.PlayerManaging;
using LeaderboardSimulator.Logic.Interfaces.Services;
using LeaderboardSimulator.Logic.Mappers;
using LeaderboardSimulator.Logic.PlayerManaging;
using LeaderboardSimulator.Logic.Services;
using LeaderboardSimulator.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("Logs/leaderboard_simulator.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices((_, services) =>
        {
            services.AddMemoryCache();
            services.AddSingleton<IGameRepository, XmlGameRepository>();
            services.AddSingleton<IGameCache, GameCache>();

            services.AddSingleton<ILeaderboardSorter, ScoreBasedSorter>();
            services.AddSingleton<IMatchPlayerManager, MatchPlayerManager>();
            services.AddSingleton<IGameFactory, GameFactory>();
            services.AddSingleton<IGameMapper, GameMapper>();
            services.AddSingleton<IGameService, GameService>();
            
            services.AddTransient<Menu>();
        })
        .Build();

    await RunApplicationAsync(host);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    await Log.CloseAndFlushAsync();
}

static async Task RunApplicationAsync(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try 
    {
        var gameService = services.GetRequiredService<IGameService>();
        var gameFactory = services.GetRequiredService<IGameFactory>();

        logger.LogInformation("Attempting to load game...");
        var game = await gameService.GetGameAsync();

        if (game == null || game.Matches.Count == 0)
        {
            logger.LogWarning("No existing game found. Creating a new simulation.");
            game = gameFactory.CreateGame(4, 10); 
            await gameService.SaveGameAsync(game);
        }
        else
        {
            logger.LogInformation("Game loaded successfully with {Count} matches.", game.Matches.Count);
        }

        var simulator = new ConcurrentMatchSimulator(game.Matches);
        var menuLogger = services.GetRequiredService<ILogger<Menu>>();
        
        var menu = new Menu(simulator, game, gameService, menuLogger);
        
        await menu.DisplayAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred properly running the application logic.");
        throw;
    }
}