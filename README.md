# SZU2025Spring-Computer-game-developing

This repository now contains a cleaner and more reusable Unity flocking / boids demo for the SZU 2025 Spring Computer Game Developing course.

## Overview

The project demonstrates a classic boids simulation with three core local behaviors:

- collision avoidance
- velocity matching
- flock centering

An additional moving attractor is used to pull or push the flock, making the motion more dynamic and easier to observe in class demos.

## Current Repository Contents

This repository is a lightweight scene-and-scripts package instead of a full Unity project. The main files are:

- `Boid.cs`: movement logic for each boid
- `Neighborhood.cs`: neighbor detection and local averaging
- `Spawner.cs`: boid spawning and runtime parameter storage
- `Attractor.cs`: moving target used to steer the flock
- `LookAtAttractor.cs`: keeps an object facing the attractor
- `_Scene_().unity`: sample scene
- `Boid.prefab`: sample boid prefab

## Improvements Included

Compared with the original version, this repository now includes:

- fixed Unity lifecycle bugs in `Neighborhood` (`Start` / `FixedUpdate` naming issues)
- safer component requirements through `RequireComponent`
- runtime null checks for missing `Spawner`, `Rigidbody`, and optional trail renderer usage
- corrected and continuously updated trigger-collider configuration for neighbor detection
- parameter clamping in the Inspector to reduce invalid settings
- coroutine-based spawning instead of string-based `Invoke`
- better default handling when `boidAnchor` is not assigned
- rewritten documentation for setup and tuning

## How To Use

1. Open Unity and create a project that uses a recent Unity version.
2. Copy these scripts, the prefab, and the sample scene into your `Assets` folder.
3. Make sure the boid prefab contains:
   - `Boid`
   - `Neighborhood`
   - `Rigidbody`
   - `SphereCollider` set as trigger or left for script configuration
4. Add a `Spawner` object to the scene and assign:
   - `boidPrefab`
   - `boidAnchor` (optional, defaults to the spawner itself)
5. Add an `Attractor` object to the scene.
6. Play the scene and tune parameters in the Inspector.

## Recommended Unity Setup

For the boid prefab:

- disable gravity on the rigidbody
- freeze rotation if physics rotation is not desired
- use a visible mesh or model
- optionally add a `TrailRenderer`

For the neighborhood collider:

- keep it on the same object as the boid
- let the script control the radius from `neighborDist`

## Key Parameters

### Spawner

- `numBoids`: total number of boids to create
- `spawnRadius`: initial random spawn radius
- `spawnDelay`: delay between boid spawns
- `velocity`: target speed of each boid
- `neighborDist`: radius used to detect nearby boids
- `collDist`: minimum distance used for avoidance
- `velMatching`: strength of alignment
- `flockCentering`: strength of cohesion
- `collAvoid`: strength of separation
- `attractPull`: attraction strength when far from the attractor
- `attractPush`: repulsion strength when too close to the attractor
- `attractPushDist`: distance threshold for switching from pull to push

### Attractor

- `radius`: movement amplitude
- `xPhase`, `yPhase`, `zPhase`: sinusoidal motion speeds on each axis

## Suggested Extensions

If you want to continue improving this course project, good next steps are:

- add boundary handling or wrap-around world space
- expose presets with `ScriptableObject`
- add UI sliders for runtime tuning
- add obstacle avoidance
- add different flock roles such as leader / follower / predator
- visualize neighbor radius and avoidance radius with gizmos

## Notes

- The scripts currently use `Rigidbody.linearVelocity`, which is suitable for newer Unity versions.
- If you use an older Unity version, replace `linearVelocity` with `velocity`.

## License

This repository currently has no explicit license. Add one before redistributing outside coursework if needed.
