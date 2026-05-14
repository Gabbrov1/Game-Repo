# Project Justification
---
This project is the furthest chain in my exploration of 3D Graphics and Voxel Engines.

The final scope of this project is a working MVP Voxel terrain generator engine that I will attempt to optimise as much as possible, with the hopes of having borderline endless looking terrains.

# Past Iterations
---
1. Unity C# Voxel engine using recursive instantiation of Primitive objects
2. Base Java 3D environment, using only the base libraries in order to learn more about how 3D graphics work.
3. Base C# 3D Raytracer in a Console Application project, only using base C# libraries.
4. (this) Voxel based terrain generator using OpenTK and C#


# MineBlox Engine

A Minecraft-inspired voxel engine built from scratch in C# using OpenGL via OpenTK. Developed as a learning project to explore game engine architecture, rendering pipelines, and performance optimisation.

---

## Technology Stack

- **Language**: C# (.NET 9)
- **Graphics API**: OpenGL 4 via OpenTK
- **Noise Generation**: SharpNoise
- **Architecture**: Multi-project solution separating engine and game logic

---

## Project Structure

```
MineBlox/
  src/
    MineBlox.Engine/         ← Core engine (windowing, rendering, world, camera)
      Core/
        Window.cs            ← Game loop, OpenGL context, debug modes
      Rendering/
        Mesh.cs              ← VAO/VBO management, GPU upload, IDisposable
        ShaderHandler.cs     ← GLSL shader compilation and linking
      Camera/
        Camera.cs            ← Position, view/projection matrices, input
        Frustum.cs           ← Frustum plane extraction and AABB testing
      World/
        Block.cs             ← Block data and IsSolid property
        BlockType.cs         ← Air, Grass, Dirt, Stone enum
        Chunk.cs             ← 16x16x16 block array, mesh ownership
        ChunkMesher.cs       ← Face culling mesh generation
    MineBlox.Game/           ← Game logic (terrain generation, entry point)
      Program.cs             ← Entry point, bootstraps engine
      WorldGen.cs            ← Procedural terrain generation
  assets/
    shaders/
      basic.vert             ← Vertex shader with MVP matrices and brightness
      basic.frag             ← Fragment shader with face shading
```

---

## Implemented Features

### Rendering
- OpenGL 4 context via OpenTK
- GLSL shader compilation with error reporting
- VAO/VBO mesh management with proper GPU resource cleanup
- Per-face brightness shading — top faces fully lit, sides medium, bottom dark
- Back-face culling via OpenGL state and winding order
- Depth testing

### Camera
- Full 3D perspective camera with configurable FOV
- Mouse look (pitch and yaw with clamp to prevent gimbal flip)
- WASD movement, Space/Shift for vertical movement
- Frustum extraction from view-projection matrix
- AABB frustum culling (chunks outside the view cone are not drawn)

### World
- 16x16x16 chunk data structure
- Procedural terrain generation using Perlin noise via SharpNoise
- Seeded noise (same seed produces identical terrain)
- Layered block types (grass surface, dirt subsurface, stone base)
- Face culling at mesh level (internal faces between solid blocks are never generated, only visible surfaces reach the GPU)
- Air block support (empty space generates no geometry)

### Debug Tools
- F3 cycles polygon mode: Fill, Wireframe, Points
- F1 cycles cursor state: Grabbed, Normal, Confined
- Configurable point size for vertex inspection

### Engine Architecture
- Clean separation between `MineBlox.Engine` and `MineBlox.Game`
- All GPU resources implement `IDisposable` with proper cleanup on unload
- Nullable reference types enabled (null safety enforced at compile time)
- SOLID principles throughout (single responsibility per class, no god objects)

---

## Planned Features

### Next Milestone — Multi-Chunk World

- `WorldManager.cs` — chunk dictionary (`Dictionary<Vector2i, Chunk>`), load/unload lifecycle
- Dynamic chunk loading as player moves, configurable render radius (default 6 chunks)
- Background chunk generation via `Task.Run()` and .NET thread pool
- `ConcurrentQueue` pipeline (mesh data built off main thread, GPU upload on main thread only)
- Cross-chunk face culling (faces on chunk borders correctly reference neighbouring chunks)
- Buffer zone (chunks generated but not rendered beyond render radius, deleted beyond buffer radius)

### Should Have

- UV mapping and texture atlas, per-face texture coordinates, single texture sheet
- Mesh deformation (add and remove blocks at runtime, dirty chunk rebuild)
- Seeded world generation (world seed passed at startup, deterministic terrain everywhere)

### Could Have

- World saving and loading (chunk serialisation to disk, load on demand)
- Basic HUD and UI (crosshair, hotbar, debug overlay with position and FPS)
- Block registry from JSON (define block types in data files, no code changes needed for new blocks, simmilar to Minecraft)

### Would Like

- Inventory system (item stacks, hotbar selection, block placement)
- Tree generation (procedural foliage as part of world gen pass)
- Chat and command system (slash commands similar to Minecraft)

### Future Considerations

- LOD system (simplified meshes for distant chunks)
- Occlusion culling (flood fill or visibility graph to skip fully hidden chunks)
- Storage system (chests, containers, item persistence)
- Multiplayer architecture (client/server separation)

---

## Threading Model

The engine is designed to avoid Minecraft Java Edition's main-thread bottleneck.

| Thread | Responsibility |
|---|---|
| Main thread | OpenGL rendering only, no exceptions |
| World thread | Terrain generation, mesh building (`float[]`) |
| IO thread | World saving, loading, JSON block registry |

Communication between threads uses `ConcurrentQueue<T>`. The world thread enqueues completed mesh data. The main thread drains the queue each frame and performs GPU uploads. No shared mutable state, no locks required. (AKA generate mesh without making the game stutter)

---

## Architecture Principles

- **Single Responsibility** — every class owns exactly one concern
- **Dependency direction** — `MineBlox.Game` depends on `MineBlox.Engine`, never the reverse
- **Resource safety** — every OpenGL handle is wrapped in `IDisposable` and freed on unload
- **Thread safety** — OpenGL context is main-thread only, all other work is parallelisable
- **Data over objects** — blocks store type data only, no geometry, no render logic

---

## Building

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run
cd src/MineBlox.Game
dotnet run
```

Requires .NET 9 SDK. OpenTK and SharpNoise are pulled from NuGet automatically.

---

## Controls

| Key | Action |
|---|---|
| W / A / S / D | Move |
| Space | Move up |
| Left Shift | Move down |
| Mouse | Look around |
| F1 | Cycle cursor mode |
| F3 | Cycle debug view (fill / wireframe / points) |
| Escape | Quit |

