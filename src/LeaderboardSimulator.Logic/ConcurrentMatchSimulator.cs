using LeaderboardSimulator.Logic.Models;

namespace LeaderboardSimulator.Logic;

public class ConcurrentMatchSimulator(List<Match> matches)
{
    private const float PercentageOfCpuLoad = 0.8f;
    private static readonly Random Random = new();

    public void Simulate(int degreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = (int)(degreeOfParallelism * PercentageOfCpuLoad)
        };
        
        if (parallelOptions.MaxDegreeOfParallelism < 1) parallelOptions.MaxDegreeOfParallelism = 1;

        Parallel.ForEach(matches, parallelOptions, match =>
        {
            Parallel.ForEach(match.Players, parallelOptions, player =>
            {
                var scores = Random.Next(1, 11);
                if (player.Name is not null)
                {
                    match.ScorePlayer(player.Name, scores);
                }
            });
        });
    }
}