# 🏆 Leaderboard Simulator

![Build Status](https://img.shields.io/github/actions/workflow/status/kuma4ka/leaderboard-simulator/dotnet.yml?branch=main)
![.NET](https://img.shields.io/badge/.NET-8.0-512bd4)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ed)
![License](https://img.shields.io/badge/License-MIT-green)

A robust, multi-threaded .NET 8 console application that simulates competitive gaming matches, calculates player scores in real-time, and manages persistent leaderboards.

Built with a focus on **Clean Architecture**, and **Design Patterns**.

---

## 🚀 Key Features

* **⚡ Concurrent Simulation:** Utilizes `Parallel.ForEach` and `PLINQ` to simulate matches across multiple CPU threads, mimicking high-load scenarios.
* **🏗️ N-Tier Architecture:** Strict separation of concerns:
    * **Presentation:** Console UI & Composition Root.
    * **Logic:** Domain models, Factories, Mappers, and Services.
    * **DataAccess:** XML persistence and Caching logic.
    * **Shared:** Common DTOs and Contracts.
* **💾 Smart Caching (Cache-Aside):** Implements the Cache-Aside pattern using `IMemoryCache` to minimize disk I/O operations.
* **🐳 Containerized:** Fully Dockerized with multi-stage builds and volume mapping for data persistence.
* **✅ Automated Testing:** Unit tests covering core logic using **xUnit** and **Moq**.
* **⚙️ CI/CD:** GitHub Actions pipeline that enforces quality gates (Test -> Build -> Docker).

---

## 🛠️ Tech Stack & Patterns

| Category | Technology / Pattern | Description |
| :--- | :--- | :--- |
| **Framework** | .NET 8, C# 12 | Core platform using latest features (File-scoped namespaces, Global usings). |
| **DI** | `Microsoft.Extensions.DependencyInjection` | Loose coupling via Constructor Injection. |
| **Data** | XML Serialization | Lightweight file-based storage. |
| **Caching** | `Microsoft.Extensions.Caching.Memory` | In-memory storage for high-speed access. |
| **Logging** | Serilog | Structured logging to Console and File. |
| **Pattern** | **Repository** | Abstraction over the Data Access Layer (`XmlGameRepository`). |
| **Pattern** | **Strategy** | Pluggable sorting algorithms (`ILeaderboardSorter`). |
| **Pattern** | **Factory** | Centralized object creation logic (`GameFactory`). |
| **Pattern** | **Decorator/Proxy** | Caching logic wraps the repository calls (`GameCache`). |

---

## 🏃 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Option 1: Run via Docker (Recommended)
The easiest way to run the application in an isolated environment.

1.  **Build and Run:**
    ```bash
    docker-compose up --build
    ```
    *(Use `docker-compose run --rm leaderboard-app` if you need an interactive terminal session).*

2.  **Persistence:**
    * Game data is saved to `./Data/game.xml` on your host machine.
    * Logs are saved to `./Logs/`.
    * These persist even after the container stops.

### Option 2: Run Locally (CLI)

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/kuma4ka/leaderboard-simulator.git
    cd leaderboard-simulator
    ```

2.  **Restore & Run:**
    ```bash
    dotnet run --project src/LeaderboardSimulator.Presentation
    ```

3.  **Run Tests:**
    ```bash
    dotnet test
    ```

---

## 🏗️ Architecture Overview

The solution follows the **Dependency Inversion Principle**:

```text
src/
├── Leaderboard.Shared/                # DTOs, Interfaces (No dependencies)
├── LeaderboardSimulator.DataAccess/   # XmlRepository, Cache (Depends on Shared)
├── LeaderboardSimulator.Logic/        # Domain Models, Services (Depends on Shared, DataAccess)
└── LeaderboardSimulator.Presentation/ # UI, DI Setup (Depends on all above)