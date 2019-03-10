# Cloth Simulation

A mass-spring-damper based real-time cloth simulation, implemented in [Unity](https://unity3d.com/).

![image](https://imgur.com/cs4wZme.gif)

The cloth is represented as a grid of nodes, and is affected by five forces:

1. Gravity.
2. Springs.
3. Wind.
4. Air resistance.
5. Collision with objects.

## Gravity

Each node is pulled down by the force of gravity.

## Springs

Each node is connected to its neighbors by a series of springs:

1. **Parallel** springs bind adjacent nodes horizontally and vertically across the grid. They help preserve the overall structure of the cloth.
2. **Diagonal** springs bind adjacent nodes diagonally across the cloth. They help prevent shearing as the cloth moves.
3. **Bending** springs are similar to diagonal springs. They bind every second node (rather than adjacent nodes) diagonally across the cloth. They help prevent excessive deformation as the cloth moves.

## Wind

Each triangle is pushed by a user-defined wind vector, proportional to the percentage of the triangle facing the wind.

## Air resistance

Each triangle also resists the air as it moves, proportional to its area.

## Collision with objects

Finally, the cloth reacts to any number of user-defined sphere colliders. These can be moved in real-time.

Collision detection is (currently) not continous, so fast moving nodes, or fast moving sphere colliders may result in missed collisions. I.e. objects may appear to "phase through" the cloth, if either are moving fast enough.