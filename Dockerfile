FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Leaderboard.Shared/Leaderboard.Shared.csproj", "src/Leaderboard.Shared/"]
COPY ["src/LeaderboardSimulator.DataAccess/LeaderboardSimulator.DataAccess.csproj", "src/LeaderboardSimulator.DataAccess/"]
COPY ["src/LeaderboardSimulator.Logic/LeaderboardSimulator.Logic.csproj", "src/LeaderboardSimulator.Logic/"]
COPY ["src/LeaderboardSimulator.Presentation/LeaderboardSimulator.Presentation.csproj", "src/LeaderboardSimulator.Presentation/"]

RUN dotnet restore "src/LeaderboardSimulator.Presentation/LeaderboardSimulator.Presentation.csproj"

COPY . .

WORKDIR "/src/src/LeaderboardSimulator.Presentation"
RUN dotnet build "LeaderboardSimulator.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeaderboardSimulator.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

USER app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "LeaderboardSimulator.Presentation.dll"]