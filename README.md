This project is the furthest chain in my exploration of 3D Graphics and Voxel Engines.

The final scope of this project is a working MVP Voxel terrain generator engine that I will attempt to optimise as much as possible, with the hopes of having borderline endless looking terrains.

So far other variations on this project, that I have completed so far include:

1. Unity C# Voxel engine using recursive instantiation of Primitive objects
2. Base Java 3D environment, using only the base libraries in order to learn more about how 3D graphics work.
3. Base C# 3D Raytracer in a Console Application project, only using base C# libraries.
4. (this) Voxel based terrain generator using OpenTK and C#


Features implemented so far:
- Cube Generation based on voxel information.
- Single mesh generation from array of vertexes.
- Rendering of visible faces instead of displaying all faces.
- Basic application of perlin noise.(requires further improvement and layering in order to achieve more realistic looking terrain)

Planned Implementations:
 - UV Mapping to cubes to allow meshing options.
 - infinite chunk generation, based on current player position.
 - Editable mesh, with differeing blocks.
 - Implementing structures such as trees.
 - Custom loader, simmilar to minecraft to allow for easy addition and edit of current blocks.

