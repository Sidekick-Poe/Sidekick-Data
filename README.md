# Sidekick-Data

Repository that hosts projects to fetch data from various sources and prepares JSON files for use in the main Sidekick application.

## Architecture Overview

The data generation pipeline produces static JSON files consumed by the Sidekick application at runtime. It combines data from multiple sources:

- **Path of Exile Trade API** — item listings, trade filters, and league data
- **poe.ninja** — economy data (exchange rates, stash values)
- **RePoE** (being retired) — stat translations, base items, item classes, and unique items
- **Sidekick GraphQL API** (replacing RePoE) — fresh game data extracted directly from Path of Exile's GGPK files

## Related Repositories

This repository is part of a family of related repositories that must be cloned as siblings:

```
C:\Repos\
├── Sidekick\                  # Main Sidekick application
│   └── src\
│       ├── Sidekick.Common\           ← referenced by Sidekick-Data
│       ├── Sidekick.Data\             ← referenced by Sidekick-Data
│       └── Sidekick.Data.Builder\     ← contains data builders (Repoe, Trade, Ninja, Stats, etc.)
├── Sidekick-Data\             # This repository — data generation CLI
│   └── src\Sidekick.Data.Cli\
└── Sidekick-GraphQL-API\      # Build-time GraphQL API for PoE1/PoE2 game data
```

### Sidekick-GraphQL-API

The **Sidekick GraphQL API** is a build-time GraphQL API that exposes fresh Path of Exile 1 and 2 game data extracted directly from the game's GGPK files. It **must be running** at `http://127.0.0.1:4000/graphql` before generating data that depends on it.

See the [Sidekick-GraphQL-API README](../Sidekick-GraphQL-API/README.md) for setup and usage instructions.

## Project Structure

```
Sidekick-Data/
├── .github/workflows/       # CI/CD: auto-runs data generation on push to main
├── data/                    # Generated output JSON files
│   ├── poe1/
│   │   ├── game/            # Built stats.json per language
│   │   ├── ninja/           # poe.ninja economy data
│   │   └── trade/           # Trade API data (filters, items, stats)
│   └── poe2/
│       ├── ninja/
│       └── trade/
├── src/
│   └── Sidekick.Data.Cli/   # Entry point — .NET CLI application
│       ├── Program.cs       # DI setup, argument parsing, orchestration
│       ├── DataBuilder.cs   # Top-level download + build coordination
│       ├── RawDataProvider.cs
│       └── ServiceCollectionExtensions.cs
├── Sidekick.Data.sln
├── Directory.Packages.props # Central package version management
└── README.md
```


## SQLite Database Architecture

The data generation pipeline uses SQLite databases as its source of truth:

| Database | Location | Purpose | Shared with Sidekick? |
|----------|----------|---------|----------------------|
| TradeApiDatabase | `data/trade.db` | Raw data from the trade API (items, stats, static, filters, leagues). Used as input for building. | No |
| SidekickDataDatabase | `data/sidekick.db` | Built and processed data. Will replace JSON files as the data source for the main Sidekick application. | Yes (future) |
| User Database | User's app data | User-specific settings and data. Unchanged. | Yes |

### Projects

```
Sidekick-Data/
├── src/
│   ├── Sidekick.Data.Trade/     ← NEW: Trade API database + downloader
│   │   ├── Data/                ← EF Core entities + DbContext
│   │   │   ├── TradeApiDatabase.cs
│   │   │   ├── TradeItem.cs
│   │   │   ├── TradeStat.cs
│   │   │   ├── TradeStaticItem.cs
│   │   │   ├── TradeFilter.cs
│   │   │   ├── TradeLeague.cs
│   │   │   └── ...
│   │   └── Download/            ← Trade API downloader
│   │       └── TradeApiDownloader.cs
│   ├── Sidekick.Data.Cli/       ← CLI entry point + builders
│   │   ├── Program.cs
│   │   ├── DataBuilder.cs
│   │   ├── Trade/               ← Trade builders (to be refactored)
│   │   ├── ItemClasses/
│   │   ├── ItemDefinitions/
│   │   ├── Stats/
│   │   └── ...
│   ├── Sidekick.Common/         ← Shared utilities
│   └── Sidekick.Data/           ← Domain models
├── data/
│   └── trade.db                 ← TradeApiDatabase (created at runtime)
└── Sidekick.Data.sln
```
## Data Generation Pipeline

The pipeline has two phases:

### 1. Download — Fetch raw data from external sources

```
--download flag
  ├── TradeDownloader     → poe.trade API → data/poe{1,2}/raw/trade/
  ├── RepoeDownloader     → RePoE (retiring) → data/poe{1,2}/raw/repoe/
  └── NinjaDownloader     → poe.ninja API → data/poe{1,2}/ninja/
```

### 2. Build — Transform raw data into application-ready JSON

```
--build flag (default: true)
  ├── LeagueBuilder          → league metadata
  ├── TradeFilterBuilder     → trade search filters
  ├── TradeStatBuilder       → trade stat definitions
  ├── StatsInvariantBuilder  → language-invariant stat details
  ├── ItemClassBuilder       → item class definitions (from RePoE/GraphQL)
  ├── ItemDefinitionBuilder  → base + unique item definitions (from RePoE/GraphQL)
  ├── PseudoBuilder          → pseudo stat calculations
  └── StatBuilder            → stat text/pattern definitions (from RePoE/GraphQL)
```

## Usage

### Running Locally

The solution expects the `Sidekick` repository as a sibling directory. The CLI project references `Sidekick.Common` and `Sidekick.Data` via relative paths.

```bash
# Build the solution (requires .NET 8.0+)
dotnet build Sidekick.Data.sln

# Run the data CLI
cd src/Sidekick.Data.Cli
dotnet run
```

### CLI Arguments

| Flag | Description |
|------|-------------|
| `--download` | Download raw data from trade, RePoE, and ninja before building |
| `--build` | Build processed JSON files (enabled by default) |
| `--items` | Build item classes and definitions |
| `--stats` | Build stat definitions |
| `--trade` | Build trade-related data (filters, stats, leagues) |
| `--repoe` | Download from RePoE |
| `--pseudo` | Build pseudo stat definitions |
| `--ninja` | Download from poe.ninja |
| `--language <code>` | Run for a specific language only (e.g., `en`, `de`) |

If no category flags are specified, all categories run by default.

### Examples

```bash
# Download everything and build all data
dotnet run -- --download

# Build only stats for English
dotnet run -- --stats --language en

# Download trade data and build trade + items
dotnet run -- --download --trade --items --build
```

## Migrating from RePoE to GraphQL API

The data builders that previously consumed RePoE JSON files are being adapted to query the GraphQL API instead. The affected builders are:

- **ItemClassBuilder** — reads `ItemClasses` table
- **ItemDefinitionBuilder** — reads `BaseItemTypes` (base items) and unique items
- **StatBuilder** — reads stat translations and `Stats`/`Mods` tables

The RePoE download step (`--repoe`) and `RepoeDownloader` class are being retired. Once migration is complete, the `--repoe` flag will be removed.

## Dependencies

| Source | Status | Data Provided |
|--------|--------|---------------|
| Path of Exile Trade API | Active | Item listings, filters, leagues, stat definitions |
| poe.ninja | Active | Economy data (exchange rates, stash values) |
| RePoE | **Retiring** | Stat translations, base items, item classes, uniques |
| Sidekick GraphQL API | **New** | Fresh PoE1/PoE2 game data from GGPK extraction |
