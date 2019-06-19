# Cloth Simulation in Unity

A mass-spring-damper based real-time cloth simulation I wrote for a class project.

![image](https://imgur.com/cs4wZme.gif)

Watch on [Youtube](https://www.youtube.com/playlist?list=PLI8z64x91TSvAmLJ2esaEwuzyoXn7Ta1p).

## About

The cloth is represented as a collection of point-masses, connected in a grid shape by springs. Each point is affected by the following five forces:

#### 1. Gravity

Gravity pulls every point in a specified direction (defaults to `(0, -9.8, 0)`).

#### 2. Springs

Each point is connected to its surrounding neighbors by a series of springs:

1. **Parallel** springs bind adjacent points horizontally and vertically across the grid. They help preserve the overall structure of the cloth.
2. **Diagonal** springs bind adjacent points diagonally across the cloth. They help prevent shearing as the cloth moves.
3. **Bending** springs are similar to diagonal springs. They bind every second point (rather than adjacent nodes) diagonally across the cloth. They help prevent excessive deformation as the cloth moves.

#### 3. Wind

#### 4. Air Resistance

The point collection forms a grid from which triangles can be extrapolated. For every point within a triangle, a force proportional to the surface area is applied relative to a specified wind direction. Air resistance proportional to the triangles area is also considered.

#### 5. Collision with objects

At every simulation step, each point checks whether it has entered a sphere collider. If it has, it reverts to its previous position. This method is not continuous, so if either the cloth or sphere collider is moving quickly enough, one might miss the collision and appear to "phase through" the other.