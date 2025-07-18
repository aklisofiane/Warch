# Warch - Unity Wargame Project

A tactical wargame built in Unity featuring a chess-board style battlefield with interactive tile-based gameplay.

## 📋 Project Overview

Warch is a Unity-based wargame that combines the strategic depth of tactical combat with the familiar 8x8 grid system of a chessboard. Players can interact with individual tiles, plan tactical movements, and engage in strategic warfare on a beautifully rendered 3D board.

## 🎮 Features

### Current Implementation
- **Interactive 8x8 Chess Board**: Fully functional tile-based game board

## 🛠️ Technical Requirements

- **Unity Version**: 2022.3 LTS or newer
- **Input System**: Unity's new Input System package
- **Platform**: Windows (currently configured)
- **Rendering**: Universal Render Pipeline (URP) compatible

- ## Setup

1. Clone the repository
2. Open the project in Unity
3. Assign materials in the WarBoard component:
   - `tileMaterial` - Light tiles
   - `tileMaterialAlt` - Dark tiles  
   - `highlightMaterial` - Hover effect
   - `borderMaterial` - Board frame
   - `groundMaterial` - Surrounding ground
4. Create "Tile" and "Hover" layers in Unity
5. Press Play to interact with the board

## Project Structure

```
Assets/
├── Scripts/
│   └── WarBoard.cs     # Main board controller
├── Materials/          # Board materials
└── Scenes/             # Game scenes
```

## Controls

- **Mouse**: Hover over tiles to see highlight effects

## Development Status

This project is in active development. Current focus is on the core board system and tile interaction mechanics.

## Future Plans

- Game piece placement and movement
- Turn-based gameplay mechanics
- Combat system
- Multiple unit types
- Campaign mode

## Contributing

Feel free to contribute to the project by submitting pull requests or reporting issues.
