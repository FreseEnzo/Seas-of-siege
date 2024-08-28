# Sieges of Sieg

Sieges of Sieg is a continuous tower defense game developed with Unity and C#. Defend your island against waves of enemies, expand your territory, and strategically place towers to survive as long as possible!

## Table of Contents

1. [Game Overview](#game-overview)
2. [Features](#features)
3. [Getting Started](#getting-started)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
4. [Gameplay](#gameplay)
   - [Grid Generation](#grid-generation)
   - [Terrain Generation](#terrain-generation)
   - [Wave System](#wave-system)
   - [Enemy Creation](#enemy-creation)
   - [Gameplay Mechanics](#gameplay-mechanics)
   - [Construction Phase](#construction-phase)
   - [Tower Types](#tower-types)
   - [Game Over Conditions](#game-over-conditions)
5. [Development](#development)
6. [License](#license)

## Game Overview

Sieges of Sieg is an exciting tower defense game where players must protect their island from relentless waves of enemies. Expand your territory, build powerful towers, and use strategy to survive as long as possible in this continuous challenge.

## Features

- Procedurally generated grid-based map
- Dynamic terrain with destructible elements
- Wave-based enemy spawning system
- Player-controlled character with auto-attack capabilities
- Three unique tower types with upgrade possibilities
- Strategic island expansion mechanics
- Resource management (coins) for purchasing towers and upgrades
- Continuous gameplay with increasing difficulty

## Getting Started

### Prerequisites

- Unity 2020.3 LTS or later
- Git (for cloning the repository)

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/FreseEnzo/Seas-of-siege.git
   ```

2. Open Unity Hub

3. Click on "Add" and browse to the cloned "Seas-of-siege" directory

4. Select the project and click "Open"

5. Once Unity loads the project, navigate to the "Scenes" folder in the Project window

6. Double-click on the main scene file (e.g., "MainGame.unity") to open it

7. Press the Play button in the Unity Editor to run the game

## Gameplay

### Grid Generation

The game starts by generating a grid-based map that defines:
- Enemy spawn points
- Initial island location
- Player character starting position

### Terrain Generation

After grid generation, the game populates the map with various assets:
- Player character
- Terrain features (trees, rocks, etc.)

### Wave System

The game operates on a wave-based system:
- Each wave consists of a set number of enemies
- Defeating all enemies in a wave allows the player to build or upgrade
- Waves become progressively more difficult

### Enemy Creation

During waves:
- Enemies spawn at predetermined points
- Each enemy has a randomly assigned visual asset
- Enemies target the nearest land and move in a straight line
- Upon reaching land, enemies attack and can destroy terrain

### Gameplay Mechanics

- The player character automatically attacks nearby enemies
- Towers attack enemies within their range
- Defeated enemies drop coins for the player to collect

### Construction Phase

After defeating a wave, players enter the construction phase:
- Build new towers
- Upgrade existing towers
- Expand the island

The player must manually end this phase to start the next wave.

### Tower Types

1. **Mcc A**
   - Basic single-target tower
   - Balanced damage and attack speed

2. **Mcc B**
   - Area-of-effect damage
   - Lower damage and range compared to Mcc A

3. **Mcc C**
   - High damage, slow attack speed
   - Longest range of all towers

All towers can be upgraded three times, improving damage, attack speed, and range.

### Game Over Conditions

The game ends when the player's current island is destroyed while they are still on it.

## Development

This project is developed using Unity and C# with an object-oriented approach. The main game loop follows these steps:

1. Grid and terrain generation
2. Wave counting and enemy spawning
3. Gameplay execution (player and tower attacks, enemy movement)
4. Construction phase between waves
5. Continuous loop until player defeat

## License

This project is licensed under the [MIT License](https://choosealicense.com/licenses/mit/).
